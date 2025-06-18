using Common.Dtos.Profile;
using Match.BusinessLogic.Services.Interfaces;
using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;

namespace Match.BusinessLogic.Services.Implementations;

public class HybridScoringService : IHybridScoringService
{
    private readonly IUnitOfWork _unitOfWork;

    public HybridScoringService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ProfileClientDto>> RankProfilesAsync(
        ProfileClientDto user, List<ProfileClientDto> candidates, CancellationToken cancellationToken)
    {
        // Получаем популярность и коллаборативную фильтрацию
        var popularityMap = await BuildPopularityMapAsync(candidates.Select(c => c.Id).ToList());
        var collaborativeScores = await GetCollaborativeFilteringScores(user, candidates, cancellationToken);

        var scored = candidates.Select(c => new
            {
                Profile = c,
                Score = ComputeContentScore(user, c) * 0.5
                        + GetPopularityScore(c.Id, popularityMap) * 0.3
                        + collaborativeScores.GetValueOrDefault(c.Id, 0) * 0.3
            })
            .OrderByDescending(x => x.Score)
            .Select(x => x.Profile)
            .ToList();

        return scored;
    }

    
    private async Task<Dictionary<long, double>> GetCollaborativeFilteringScores(
    ProfileClientDto user,
    List<ProfileClientDto> candidates,
    CancellationToken cancellationToken)
    {
        var collaborativeScores = new Dictionary<long, double>();

        // 1. Получаем, кого лайкал пользователь
        var userOutgoingLikes = await _unitOfWork.Likes.GetAsync(
            l => l.ProfileId == user.Id, cancellationToken);

        var userLikedIds = userOutgoingLikes
            .Select(l => l.TargetProfileId)
            .ToList();

        if (!userLikedIds.Any())
            return candidates.ToDictionary(c => c.Id, c => 0.0);

        // 2. Получаем, кто лайкал тех, кого пользователь уже лайкал
        var likedProfilesIncomingLikes = await _unitOfWork.Likes.GetAsync(
            l => userLikedIds.Contains(l.TargetProfileId), cancellationToken);

        // Группируем: TargetProfileId -> Set of liker Ids
        var likedProfilesLikerMap = likedProfilesIncomingLikes
            .GroupBy(l => l.TargetProfileId)
            .ToDictionary(g => g.Key, g => g.Select(l => l.ProfileId).ToHashSet());

        // 3. Кандидаты, которых нужно оценить
        foreach (var candidate in candidates)
        {
            // Получаем, кто лайкал кандидата
            var candidateIncomingLikes = await _unitOfWork.Likes.GetAsync(
                l => l.TargetProfileId == candidate.Id, cancellationToken);

            var candidateLikerIds = candidateIncomingLikes
                .Select(l => l.ProfileId)
                .ToHashSet();

            if (candidateLikerIds.Count == 0)
            {
                collaborativeScores[candidate.Id] = 0;
                continue;
            }

            // Сравниваем с каждым лайкнутым пользователем
            double totalSimilarity = 0;
            int count = 0;

            foreach (var likedProfileId in userLikedIds)
            {
                if (!likedProfilesLikerMap.TryGetValue(likedProfileId, out var likers))
                    continue;

                int common = likers.Intersect(candidateLikerIds).Count();

                if (common > 0)
                {
                    double similarity = common / (Math.Sqrt(likers.Count) * Math.Sqrt(candidateLikerIds.Count));
                    totalSimilarity += similarity;
                    count++;
                }
            }

            collaborativeScores[candidate.Id] = count > 0 ? totalSimilarity / count : 0;
        }

        return collaborativeScores;
    }
    
    private double ComputeContentScore(ProfileClientDto user, ProfileClientDto candidate)
    {
        double score = 0;

        score += user.Goal!= null && candidate.Goal != null && user.Goal.Id == candidate.Goal.Id ? 10 : 0;
        score += user.Languages.Intersect(candidate.Languages).Count() * 3;
        score += user.Interests.Intersect(candidate.Interests).Count() * 2;

        return score;
    }

    private async Task<Dictionary<long, int>> BuildPopularityMapAsync(IEnumerable<long> profileIds)
    {
        var likes = await _unitOfWork.Likes.GetLikesCountAsync(profileIds);
        return likes.ToDictionary(l => l.ProfileId, l => l.Count);
    }

    private double GetPopularityScore(long profileId, Dictionary<long, int> popularityMap)
    {
        return popularityMap.TryGetValue(profileId, out var score) ? score : 0;
    }
    
    private double CalculateCosineSimilarity(IEnumerable<Like> likesA, IEnumerable<Like> likesB)
    {
        // Преобразуем лайки в наборы ID профилей
        var likesAIds = likesA.Select(l => l.TargetProfileId).ToList();
        var likesBIds = likesB.Select(l => l.TargetProfileId).ToList();

        // Находим пересечение лайков
        var commonLikes = likesAIds.Intersect(likesBIds).Count();

        // Если нет общих лайков, возвращаем 0
        if (commonLikes == 0)
            return 0;

        // Если есть общие лайки, можно посчитать схожесть
        return (double)commonLikes / (Math.Sqrt(likesAIds.Count) * Math.Sqrt(likesBIds.Count));
    }

}
