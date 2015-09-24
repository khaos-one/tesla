namespace Tesla.SocialApi.Vk {
    public interface IVkApi {
        AuthorizationResult Authorize(string username, string password);
    }
}