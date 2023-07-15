using UnityEngine;

public class Shop : Interactable
{
    [Header("Shop Properties")]
    [SerializeField] private int adding;
    [SerializeField] private ShopType type;

    [SerializeField] private GameObject ruText;
    [SerializeField] private GameObject engText;
    public override void Interaction(PlayerData playerData) 
    {
        int allmoney = GameManager.Instance.GetMoney();

        if (cost <= allmoney)
        {
            GameManager.Instance.SetMoney(allmoney - cost);
            EventManager.Instance.OnSetMoney(GameManager.Instance.GetMoney());

            switch (type)
            {
                case ShopType.Patrons:

                    playerData.getWeapon().AddPatron(adding);
                    break;

                case ShopType.Grenade:
                    playerData.addGrenade(adding);
                    break;

                case ShopType.Mine:
                    playerData.addMine(adding);
                    break;
            }
        }
    }

    public void changeLanguage(Langauge langauge)
    {
        switch (langauge)
        {
            case Langauge.Russian:
                ruText.SetActive(true);
                engText.SetActive(false);
                break;
            case Langauge.English:
                ruText.SetActive(false);
                engText.SetActive(true);
                break;
        }
    }

    public ShopType GetShopType() => type;
}

public enum ShopType{
    Patrons,
    Grenade,
    Mine
}
