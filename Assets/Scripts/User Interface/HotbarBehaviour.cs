using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class HotbarBehaviour : MonoBehaviour {
    public Sprite slotIcon;
    public Sprite selectedSlotIcon;

    private HotbarSlot[] hotbarSlots;
    public int selectedSlot = 0;
    private int mouseWheelPosition;

    public PlayerController player;

    // Start is called before the first frame update
    void Start() {
        hotbarSlots = gameObject.GetComponentsInChildren<HotbarSlot>();
    }

    // Update is called once per frame
    void Update() {
        if (player.isInventoryOpen) return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            mouseWheelPosition += 1;
            selectedSlot = (mouseWheelPosition % 5);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedSlot = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            selectedSlot = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            selectedSlot = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            selectedSlot = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            selectedSlot = 4;
        }

        foreach (var slot in hotbarSlots) {
            if (slot == hotbarSlots[selectedSlot] && slot.selectedItem != null) {
                slot.GetComponentInChildren<Image>().sprite = selectedSlotIcon;
                player.selectedTool = slot.selectedItem;
            }
            else {
                slot.GetComponentInChildren<Image>().sprite = slotIcon;
            }
        }
    }
}