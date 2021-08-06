using System.Collections.Generic;

public static class Constants
{
    public static class GameStates {
        public const string INTRO = "Intro";
        public const string REQUEST = "Request";
        public const string PLAY = "Play";
        public const string SUCCESS = "Success";
    }

    public static Dictionary<RequestCategory, string> Requests = new Dictionary<RequestCategory, string>() {
        { RequestCategory.Produce, "produce" },
        { RequestCategory.Dessert, "desserts" },
        { RequestCategory.Junk, "junk foods" },
        { RequestCategory.Healthy, "healthy foods" },
        { RequestCategory.Raw, "raw foods" }
    };
}
