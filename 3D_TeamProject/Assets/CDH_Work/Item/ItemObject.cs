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
        var player = CharacterManager.Instance.Player;

        player.itemData = data;
        player.addItem?.Invoke();


        if (data.type == ItemType.Consumable)
        {
            foreach (var c in data.consumables)
            {
                switch (c.type)
                {
                    case ConsumableType.Hunger:
                        player.condition.Eat(c.value);
                        Debug.Log($"[회복됨] {c.type} + {c.value}");
                        break;
                    case ConsumableType.Drink:
                        player.condition.Drink(c.value);
                        break;
                    case ConsumableType.Health:
                        player.condition.Heal(c.value);
                        break;
                }
            }
        }
        Destroy(gameObject);
    }
}
