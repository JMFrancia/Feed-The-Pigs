using System.Collections.Generic;
using UnityEngine;

/*
 * SFXManager -> manages playing SFX directly, managing audiosources, etc.
 * DialogueManager -> Loads and plays dialogue scriptable objects
 * ConvoManager -> Loads context-specific dialogues on request, such as category intros, right/wrong answers, etc.
 *
 * Yes, I know it's confusing. 
 */
[RequireComponent(typeof(DialogueManager))]
public class ConvoManager : MonoBehaviour
{
    Dictionary<RequestCategory, string> _categoryIntros;
    Dictionary<RequestCategory, string> _categoryOutros;
    Dictionary<RequestCategory, string> _specificWrongChoiceItems;
    Dictionary<FoodType, string> _jokeItems;

    DialogueManager _dialogueManager;

    /*
     * Play game intro convo
     * Returns the SFX ID
     */
    public int PlayGameIntro(System.Action callback = null) {
        return _dialogueManager.PlayDialogue(Constants.Convos.MISC_INTRO, callback);
    }

    /*
     * Play game exit convo
     * Returns the SFX ID
     */
    public int PlayGameExit(System.Action callback = null) {
        return _dialogueManager.PlayDialogue(Constants.Convos.MISC_EXIT, callback);
    }

    /*
     * Play idle convo
     * Returns the SFX ID
     */
    public int PlayIdle(System.Action callback = null) {
        return _dialogueManager.PlayDialogue(Constants.Convos.MISC_IDLE, callback);
    }

    /*
     * Play specific wrong item convo
     * Returns the SFX ID
     */
    public int PlaySpecificWrong(RequestCategory category, System.Action callback = null) {
        return _dialogueManager.PlayDialogue(_specificWrongChoiceItems[category], callback);
    }

    /*
     * Play joke choice item convo
     * Returns the SFX ID
     */
    public int PlayJokeChoice(FoodType type, System.Action callback = null) {
        return _dialogueManager.PlayDialogue(_jokeItems[type], callback);
    }

    /*
     * Plays intro dialogue for given category
     */
    public int PlayCategoryIntro(RequestCategory category, System.Action callback = null)
    {
        return _dialogueManager.PlayDialogue(_categoryIntros[category], callback);
    }

    /*
     * Plays intro dialogue for given category
     */
    public int PlayCategoryOutro(RequestCategory category, System.Action callback = null)
    {
        return _dialogueManager.PlayDialogue(_categoryOutros[category], callback);
    }

    public int PlayCorrect(System.Action callback = null)
    {
        return _dialogueManager.PlayDialogue(Constants.Convos.MISC_CORRECT, callback);
    }

    public int PlayWrong(System.Action callback = null)
    {
        return _dialogueManager.PlayDialogue(Constants.Convos.MISC_WRONG, callback);
    }

    private void Awake()
    {
        _categoryIntros = new Dictionary<RequestCategory, string>() {
            { RequestCategory.Dessert, Constants.Convos.DESSERT_INTRO },
            { RequestCategory.Healthy, Constants.Convos.HEALTHY_INTRO },
            { RequestCategory.Junk, Constants.Convos.JUNK_INTRO },
            { RequestCategory.Vegetarian, Constants.Convos.VEGETARIAN_INTRO },
            { RequestCategory.Raw, Constants.Convos.RAW_INTRO }
        };

        _categoryOutros = new Dictionary<RequestCategory, string>() {
            { RequestCategory.Dessert, Constants.Convos.DESSERT_OUTRO },
            { RequestCategory.Healthy, Constants.Convos.HEALTHY_OUTRO },
            { RequestCategory.Junk, Constants.Convos.JUNK_OUTRO },
            { RequestCategory.Vegetarian, Constants.Convos.VEGETARIAN_OUTRO },
            { RequestCategory.Raw, Constants.Convos.RAW_OUTRO }
        };

        _specificWrongChoiceItems = new Dictionary<RequestCategory, string>() {
            { RequestCategory.Dessert, Constants.Convos.DESSERT_CARROT },
            { RequestCategory.Healthy, Constants.Convos.HEALTHY_FRIES },
            { RequestCategory.Junk, Constants.Convos.JUNK_GRAPES },
            { RequestCategory.Vegetarian, Constants.Convos.VEGETARIAN_DRUMSTICK },
            { RequestCategory.Raw, Constants.Convos.RAW_CAKE }
        };

        _jokeItems = new Dictionary<FoodType, string>() {
            { FoodType.Shoe, Constants.Convos.ITEM_SHOE },
            { FoodType.Lightbulb, Constants.Convos.ITEM_LIGHTBULB },
            { FoodType.Bone, Constants.Convos.ITEM_BONE }
        };

        _dialogueManager = GetComponent<DialogueManager>();
    }

}
