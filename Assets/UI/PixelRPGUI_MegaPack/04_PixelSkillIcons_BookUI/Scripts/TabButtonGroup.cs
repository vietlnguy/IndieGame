using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Indigolay
{
    /// <summary>
    /// Manages a group of tab buttons, ensuring only one is selected at a time
    /// </summary>
    public class TabButtonGroup : MonoBehaviour
    {
        [Header("Tab Buttons")]
        [SerializeField] private List<TabButton> tabButtons = new List<TabButton>();

        [Header("Events")]
        public UnityEvent<int> OnTabChanged;

        private TabButton selectedTab;

        private void Start()
        {
            // Register all tab buttons with this group
            foreach (TabButton button in tabButtons)
            {
                if (button != null)
                {
                    button.TabGroup = this;
                }
            }
        }

        /// <summary>
        /// Add a tab button to the group
        /// </summary>
        public void Subscribe(TabButton button)
        {
            if (!tabButtons.Contains(button))
            {
                tabButtons.Add(button);
                button.TabGroup = this;
            }
        }

        /// <summary>
        /// Remove a tab button from the group
        /// </summary>
        public void Unsubscribe(TabButton button)
        {
            if (tabButtons.Contains(button))
            {
                tabButtons.Remove(button);
            }
        }

        /// <summary>
        /// Called when mouse enters a tab button
        /// </summary>
        public void OnTabEnter(TabButton button)
        {
            if (button != selectedTab)
            {
                button.SetHover(true);
            }
        }

        /// <summary>
        /// Called when mouse exits a tab button
        /// </summary>
        public void OnTabExit(TabButton button)
        {
            if (button != selectedTab)
            {
                button.SetHover(false);
            }
        }

        /// <summary>
        /// Called when a tab button is clicked
        /// </summary>
        public void OnTabSelected(TabButton button)
        {
            if (selectedTab == button) return;

            // Deselect previous tab
            if (selectedTab != null)
            {
                selectedTab.SetSelected(false);
            }

            // Select new tab
            selectedTab = button;
            selectedTab.SetSelected(true);

            // Invoke event with tab index
            int tabIndex = tabButtons.IndexOf(button);
            OnTabChanged?.Invoke(tabIndex);
        }

        /// <summary>
        /// Check if a tab button is currently selected
        /// </summary>
        public bool IsSelected(TabButton button)
        {
            return selectedTab == button;
        }

        /// <summary>
        /// Programmatically select a tab by index
        /// </summary>
        public void SelectTab(int index)
        {
            if (index >= 0 && index < tabButtons.Count)
            {
                OnTabSelected(tabButtons[index]);
            }
        }

        /// <summary>
        /// Get the currently selected tab index
        /// </summary>
        public int GetSelectedIndex()
        {
            return selectedTab != null ? tabButtons.IndexOf(selectedTab) : -1;
        }
    }
}
