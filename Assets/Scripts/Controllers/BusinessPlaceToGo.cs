using UnityEngine;

public class BusinessPlaceToGo : PlaceToGo
{
    [SerializeField] private BusinessBehaviour _businessBehaviour;

    protected override void Start()
    {

    }

    private void OnEnable()
    {
        _businessBehaviour.OnActivated += Bla;
    }

    private void OnDisable()
    {
        _businessBehaviour.OnActivated -= Bla;
    }

    private void Bla()
    {
        SendPlace();
    }
}