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

    public static class Convos {
        public static string DESSERT_CARROT = "Dessert_carrot";
        public static string DESSERT_INTRO = "Dessert_intro";
        public static string DESSERT_OUTRO = "Dessert_outro";
        public static string HEALTHY_FRIES = "Healthy_frenchfries";
        public static string HEALTHY_INTRO = "Healthy_intro";
        public static string HEALTHY_OUTRO = "Healthy_outro";
        public static string ITEM_BONE = "Item_bone";
        public static string ITEM_LIGHTBULB = "Item_lightbulb";
        public static string ITEM_SHOE = "Item_shoe";
        public static string JUNK_GRAPES = "Junk_grapes";
        public static string JUNK_INTRO = "Junk_intro";
        public static string JUNK_OUTRO = "Junk_outro";
        public static string MISC_CORRECT = "Misc_correct";
        public static string MISC_INTRO = "Misc_intro";
        public static string MISC_EXIT = "Misc_exit";
        public static string MISC_IDLE = "Misc_idle";
        public static string MISC_WRONG = "Misc_wrong";
        public static string RAW_CAKE = "Raw_cake";
        public static string RAW_INTRO = "Raw_intro";
        public static string RAW_OUTRO = "Raw_outro";
        public static string VEGETARIAN_DRUMSTICK = "Vegetarian_drumstick";
        public static string VEGETARIAN_INTRO = "Vegetarian_intro";
        public static string VEGETARIAN_OUTRO = "Vegetarian_outro";
    }
}
