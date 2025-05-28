using UnityEngine;

public interface IInteractable
{
    public string GetInteractablePrompt();      //      상호작용 가능한 오브젝트에 표시할 프롬프트를 반환하는 함수
    public void OnInteract();       //      상호작용이 발생했을 때 실행되는 함수
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractablePrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
