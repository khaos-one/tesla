namespace Tesla.SocialApi.Vk {
    public enum AuthorizationResult
        : byte {
        Unknown,
        Ok,
        UnknownFailure,
        CredentialsIncorrect,
        CaptchaEngaged
    }
}