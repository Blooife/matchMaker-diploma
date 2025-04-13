export const base = "https://localhost:5000";

export const authEndpoints = {
  login: `${base}/auth/login`,
  register: `${base}/auth/register`,
  refresh: `${base}/auth/refresh`
}

export const rolesEndpoints = {
  assignment: `${base}/roles/assignment`,
  removal: `${base}/roles/removal`,
}

export const usersEndpoints = {
  users: `${base}/users`,
  paginatedUsers(pageSize: string, pageNumber: string){
    return `${base}/users/paginated?pageSize=${pageSize}&pageNumber=${pageNumber}`
  }
}

export const profilesEndpoints = {
  profiles: `${base}/profiles`,
  byUserId(userId:string) {
    return `${base}/profiles/user/${userId}`
  },
  location: `${base}/profiles/location`,
  recs(profileId: string){
    return `${base}/profiles/${profileId}/recommendations`
  }
}

export const languagesEndpoints = {
  languages: `${base}/languages`,
  profilesLanguages: `${base}/languages/profile`
}

export const educationEndpoints = {
  educations: `${base}/educations`,
  profilesEducation: `${base}/educations/profile`
}

export const interestsEndpoints = {
  interests: `${base}/interests`,
  profilesInterests: `${base}/interests/profile`
}

export const citiesEndpoints = {
  cities: `${base}/cities`,
}

export const imagesEndpoints = {
  images: `${base}/images`,
  file(id: number){
    return `${base}/images/file/${id}`;
  }
}

export const countriesEndpoints = {
  countries: `${base}/countries`,
  cities(countryId: number) {
    return `${base}/countries/${countryId}/cities`
  }
}

export const goalsEndpoints = {
  goals: `${base}/goals`,
}

export const matchesEndpoints = {
  matches: `${base}/matches`,
  paged(pageSize: string, pageNumber: string, profileId: string){
    return `${matchesEndpoints.matches}/paged/profiles/${profileId}?pageSize=${pageSize}&pageNumber=${pageNumber}`
  }
}

export const likesEndpoints = {
  likes: `${base}/likes`
}

export const chatsEndpoints = {
  chats: `${base}/chats`,
  chatsByIds(firstProfileId: string, secondProfileId: string){
    return `${chatsEndpoints.chats}/profiles?firstProfileId=${firstProfileId}&secondProfileId=${secondProfileId}`
  },
  paged(pageSize: string, pageNumber: string, profileId: string){
    return `${chatsEndpoints.chats}/paged/profiles/${profileId}?pageSize=${pageSize}&pageNumber=${pageNumber}`
  }
}

export const messagesEndpoints = {
  messages: `${base}/messages`,
  paged(pageSize: string, pageNumber: string, chatId: string){
    return `${messagesEndpoints.messages}/paged/${chatId}?pageSize=${pageSize}&pageNumber=${pageNumber}`
  }
}
