using UnityEngine;

public class ShopPatrons : Interactable
{
    [Header("Shop Properties")]
    [SerializeField] private int addingPatron;
    public override void Interaction(Weapon weapon) 
    {
        int allmoney = GameManager.Instance.GetMoney();

        if (cost <= allmoney)
        {
            GameManager.Instance.SetMoney(allmoney - cost);
            weapon.AddPatron(addingPatron);
            Debug.Log("interaction");
            EventManager.Instance.OnSetMoney(GameManager.Instance.GetMoney());
        }
    }
}
