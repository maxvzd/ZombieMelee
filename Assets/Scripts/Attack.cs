using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Attack : MonoBehaviour
{
    public Animator animator;
    public bool IsWeaponRaised { get; private set; }
    private bool _isSwingingWeapon;
    private DealDamage _weapon;
    [SerializeField] private MultiAimConstraint armAimConstraint;
    private HolsterUnHolsterWeapon _holsterUnHolsterScript;

    // Update is called once per frame
    void Update()
    {
        if (ReferenceEquals(_holsterUnHolsterScript, null) || !_holsterUnHolsterScript.IsWeaponEquipped || ReferenceEquals(_weapon, null)) return;
        
        if (Input.GetMouseButton(0) && !_isSwingingWeapon)
        {
            IsWeaponRaised = true;
            _weapon.ReadyWeaponForSwing();
        }
        else if(IsWeaponRaised && !_isSwingingWeapon)
        {
            IsWeaponRaised = false;
            _isSwingingWeapon = true;
            _weapon.WeaponSwingAudioSource.Play();
            armAimConstraint.weight = 0.5f;
            StartCoroutine(WeaponSwingCooldown());
        }
        animator.SetBool(Constants.IsMouseHeld, IsWeaponRaised);
    }

    private void Awake()
    {
        _weapon = GetComponentInChildren<DealDamage>();
        _holsterUnHolsterScript = GetComponentInChildren<HolsterUnHolsterWeapon>();
        armAimConstraint.weight = 0f;
    }

    private IEnumerator WeaponSwingCooldown()
    {
        yield return new WaitForSeconds(_weapon.WeaponSwingTime);
        
        armAimConstraint.weight = 0f;
        _isSwingingWeapon = false;
    } 
}
