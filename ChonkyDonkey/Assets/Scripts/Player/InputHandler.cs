using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : Singleton<InputHandler>
{
    public float dir {
        get;
        private set;
    }

    private ButtonState m_move;
    public ButtonState move {
        get{return m_move;}
    }

    private ButtonState m_interact;
    public ButtonState interact {
        get {return m_interact;}
    }

    private ButtonState m_menu;
    public ButtonState menu {
        get {return m_menu;}
    }

    private void FixedUpdate() {
        this.m_move.Reset();
        this.m_interact.Reset();
        this.m_menu.Reset();
    }

    public void Move(InputAction.CallbackContext ctx) {
        this.dir = ctx.ReadValue<float>();
        this.m_move.Set(ctx);
    }

    public void Interact(InputAction.CallbackContext ctx) {
        this.m_interact.Set(ctx);
    }

    public void Menu(InputAction.CallbackContext ctx) {
        this.m_menu.Set(ctx);
    }

    public struct ButtonState {
        private bool firstFrame;
        public bool down {
            get;
            private set;
        }
        public bool pressed {
            get {
                return down && firstFrame;
            }
        }
        public bool released {
            get {
                return !down && firstFrame;
            }
        }

        public void Set(InputAction.CallbackContext ctx) {
            down = !ctx.canceled;             
            firstFrame = true;
        }
        public void Reset() {
            firstFrame = false;
        }
    }
}
