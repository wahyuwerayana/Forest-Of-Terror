/// <summary>
/// WeaponSystem.cs
/// Author: MutantGopher
/// This script manages weapon switching.  It's recommended that you attach this to a parent GameObject of all your weapons, but this is not necessary.
/// This script allows the player to switch weapons in two ways, by pressing the numbers corresponding to each weapon, or by scrolling with the mouse.
/// </summary>

using UnityEngine;
using Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;

public class WeaponSystem : MonoBehaviour
{
	public List<GameObject> weapons;				// The array that holds all the weapons that the player has
	public int startingWeaponIndex = 0;			// The weapon index that the player will start with
	private int weaponIndex;					// The current index of the active weapon

	[SerializeField] private TMP_Text ammoText;
	[SerializeField] private CanvasGroup reloadOverlay;

	public static WeaponSystem Instance;

	private void Awake() {
		if (Instance == null) {
			Instance = this;
		}
	}

	// Use this for initialization
	void Start()
	{
		// Make sure the starting active weapon is the one selected by the user in startingWeaponIndex
		weaponIndex = startingWeaponIndex;
		SetActiveWeapon(weaponIndex);
	}
	
	// Update is called once per frame
	void Update()
	{
		// Allow the user to instantly switch to any weapon
		if (Input.GetButtonDown("Weapon 1"))
			SetActiveWeapon(0);
		if (Input.GetButtonDown("Weapon 2"))
			SetActiveWeapon(1);
		if (Input.GetButtonDown("Weapon 3"))
			SetActiveWeapon(2);
		if (Input.GetButtonDown("Weapon 4"))
			SetActiveWeapon(3);
		if (Input.GetButtonDown("Weapon 5"))
			SetActiveWeapon(4);
		if (Input.GetButtonDown("Weapon 6"))
			SetActiveWeapon(5);
		if (Input.GetButtonDown("Weapon 7"))
			SetActiveWeapon(6);
		if (Input.GetButtonDown("Weapon 8"))
			SetActiveWeapon(7);
		if (Input.GetButtonDown("Weapon 9"))
			SetActiveWeapon(8);

		// Allow the user to scroll through the weapons
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
			NextWeapon();
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
			PreviousWeapon();
	}

	public void SetActiveWeapon(int index)
	{
		if(CheckWeaponReloading()){
			return;
		}

		// Make sure this weapon exists before trying to switch to it
		if (index >= weapons.Count || index < 0)
		{
			Debug.LogWarning("Tried to switch to a weapon that does not exist.  Make sure you have all the correct weapons in your weapons array.");
			return;
		}

		// Send a messsage so that users can do other actions whenever this happens
		SendMessageUpwards("OnEasyWeaponsSwitch", SendMessageOptions.DontRequireReceiver);

		// Make sure the weaponIndex references the correct weapon
		weaponIndex = index;

		// Make sure beam game objects aren't left over after weapon switching
		weapons[index].GetComponent<Weapon>().StopBeam();

		// Start be deactivating all weapons
		for (int i = 0; i < weapons.Count; i++)
		{
			weapons[i].SetActive(false);
		}

		// Activate the one weapon that we want
		weapons[index].SetActive(true);

		//StartCoroutine(RefreshAmmo(weaponIndex));

		Weapon currentWeaponScript = weapons[weaponIndex].GetComponent<Weapon>();
		UpdateAmmoText(currentWeaponScript.currentAmmo, currentWeaponScript.reservedAmmo, currentWeaponScript.unlimitedMagazine, currentWeaponScript.type);
	}

	public void NextWeapon()
	{
		if(CheckWeaponReloading()){
			return;
		}

		weaponIndex++;
		if (weaponIndex > weapons.Count - 1)
			weaponIndex = 0;
		SetActiveWeapon(weaponIndex);
	}

	public void PreviousWeapon()
	{
		if(CheckWeaponReloading()){
			return;
		}

		weaponIndex--;
		if (weaponIndex < 0)
			weaponIndex = weapons.Count - 1;
		SetActiveWeapon(weaponIndex);
	}

	public void UpdateAmmoText(int currentAmmo, int reservedAmmo, bool unlimitedMagazine, WeaponType weaponType){
		if(unlimitedMagazine){
			ammoText.text = currentAmmo.ToString() + " / INF";
		} else if(weaponType == WeaponType.Beam){
			ammoText.text = currentAmmo.ToString();
		} else{
			ammoText.text = currentAmmo.ToString() + " / " + reservedAmmo.ToString();
		}
	}

	public void SetActiveReloadOverlay(float endValue){
        reloadOverlay.DOFade(endValue, 0.5f);
	}

	private bool CheckWeaponReloading(){
		Weapon weaponScript = weapons[weaponIndex].GetComponent<Weapon>();
		return weaponScript.isReloading;
	}
}
