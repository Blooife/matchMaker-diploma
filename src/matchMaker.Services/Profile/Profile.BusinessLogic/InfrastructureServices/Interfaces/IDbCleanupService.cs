namespace Profile.BusinessLogic.InfrastructureServices.Interfaces;

public interface IDbCleanupService
{
    void DeleteOldRecords(List<long> ids);
}