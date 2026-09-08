    using UnityEngine;
    using System.Collections.Generic;
    using System.Collections;
    using TMPro;
    public class GainAttack : MonoBehaviour {

        public GameObject newAttackBox;
        public TextMeshProUGUI newAttackBoxName;
        public TextMeshProUGUI newAttackBoxText;
        public AudioSource gainedNewAttackAudio;

        
        /*
        private IEnumerator ShouldGainAttack()
        {
            active = false;
            CampPlayerController characterScript = characterSelected.GetComponent<CampPlayerController>();
            AttackMoves newAttack = null;
        
            if (sceneName.Contains("Astrid"))
            {
                if (sceneName == "Astrid1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Power Draw", "physical", 1.5f, 1.0f, 90, 0, 4, "Shoot a powerful shot at the enemy.");
                    characterScript.knownAttacks.Add(newAttack);
                }

                else if (sceneName == "Astrid2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Ankle Snare", "physical", 1.1f, 1.0f, 75, 0, 6, new List<Debuff>(){new Debuff("Crippled", 100, 1)}, "Target the enemies footing. 100% chance to cripple.");
                    characterScript.knownAttacks.Add(newAttack); 
                }

                else if (sceneName == "Astrid3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Headshot", "physical", 1.5f, 1.0f, 60, 100, 10, "Strike with extreme precision. Always crits.");
                    characterScript.knownAttacks.Add(newAttack); 
                }
            }
            else if (sceneName.Contains("Lucas"))
            {
                if (sceneName == "Lucas1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Unseen Fist", "physical", 1.1f, 1.0f, 75, 0, 6, new List<Debuff>{new Debuff("Confused", 50, 2)}, "Confuse the enemy with a flurry of strikes. 50% chance to confuse for 2 turns.");                
                    characterScript.knownAttacks.Add(newAttack);
                }

                else if (sceneName == "Lucas2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Underdog Spirit", "physical", 1.5f, 1.0f, 75, 0, 6, "Deals double damage if user is below 50% Max HP.");
                    characterScript.knownAttacks.Add(newAttack);  
                }

                else if (sceneName == "Lucas3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Killer Instinct", "physical", 2f, 1.0f, 85, 0, 8, new List<Buff>(){new Buff("Flowing", 100, 2)}, "Enter a flow state.");
                    characterScript.knownAttacks.Add(newAttack); 
                }
            }
            else if (sceneName.Contains("Celeste"))
            {
                if (sceneName == "Celeste1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new SupportMove("Mana Restore", 4, "mana", 5, "Restores mana to target. Scales with INT.");
                    characterScript.knownAttacks.Add(newAttack);
                }

                else if (sceneName == "Celeste2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new SupportMove("Cure", 4, "hp", 10, new List<string>(){"all"}, "Restores mana to target. Scales with INT.");
                    characterScript.knownAttacks.Add(newAttack);  
                }

                else if (sceneName == "Celeste3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new SupportMove("Ilvera's Protection", 10, "both", 1000, new List<Buff>(){new Buff("Blessed", 100, 1)}, "Call on the goddess to protect an ally.");
                    characterScript.knownAttacks.Add(newAttack); 
                }
            }
            else if (sceneName.Contains("Gerard"))
            {
            if (sceneName == "Gerard1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Triumphant Shout", "physical", 1.5f, 1.0f, 75, 0, 6, new List<Debuff>(){new Debuff("Taunted", 75, 1)}, "Taunts the enemy. Forced to attack closest.");
                    characterScript.knownAttacks.Add(newAttack);
                }

                else if (sceneName == "Gerard2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Shield Bash", "physical", 1.3f, 1.0f, 90, 0, 4, "Does bonus damage based on user's DEF.");
                    characterScript.knownAttacks.Add(newAttack);  
                }

                else if (sceneName == "Gerard3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Last Stand", "physical", 1.5f, 1.0f, 60, 100, 10, new List<Buff>(){new Buff("Undying", 100, 1)}, "Make a heroic last stand.");
                    characterScript.knownAttacks.Add(newAttack); 
                } 
            }
            else if (sceneName.Contains("Penelope"))
            {
            if (sceneName == "Penelope1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    //newAttack = new SupportMove("Split Chord", 2, "both", 4, "Strum your harp to restore hp and mana to an ally. Scales with INT.");
                    newAttack = new SupportMove("Power Chord", 5, "neither", 0, new List<Buff>(){new Buff("Charged", 100, 1)}, "Strum a gnarly chord. Your ally's next attack will do double damage.");
                    characterScript.knownAttacks.Add(newAttack);
                }

                else if (sceneName == "Penelope2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new SupportMove("Rejuvenate", 4, "neither", 0, "Energize your ally with the sound of music. They may take another turn.");
                    characterScript.knownAttacks.Add(newAttack);  
                }

                else if (sceneName == "Penelope3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new SupportMove("Crecendo", 8, "neither", 0, new List<Buff>(){new Buff("Invigorated", 100, 2)}, "Inspire your ally with angelic music. Boosts all primary stats.");
                    characterScript.knownAttacks.Add(newAttack); 
                } 
            }
            else if (sceneName.Contains("Katherine"))
            {
            if (sceneName == "Katherine1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Flank", "physical", 1.5f, 1.0f, 90, 0, 4, "Does bonus damage to enemies that can't attack back.");
                    characterScript.knownAttacks.Add(newAttack);
                }

                else if (sceneName == "Katherine2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Flame Charge", "magical", 1.5f, 1.0f, 75, 0, 6, "Resisted by RES rather than DEF.");
                    characterScript.knownAttacks.Add(newAttack);  
                }

                else if (sceneName == "Katherine3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Stampede", "physical", 1.5f, 1.0f, 85, 0, 6, "Does not end turn.");
                    characterScript.knownAttacks.Add(newAttack); 
                } 
            }
            else if (sceneName.Contains("Ivy"))
            {
            if (sceneName == "Ivy1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Noxious Fumes", "magical", 1.0f, 1.2f, 90, 0, 4, new List<Debuff>(){new Debuff("Poisoned", 75, 5)}, "Summon toxic fumes to poison the enemy. 75% chance to poison.");
                    characterScript.knownAttacks.Add(newAttack);
                }

                else if (sceneName == "Ivy2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Essence Drain", "magical", 1.0f, 1.5f, 75, 0, 5, "Restore mana equal to damage dealt.");
                    characterScript.knownAttacks.Add(newAttack);  
                }

                else if (sceneName == "Ivy3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Spore Burst", "magical", 1.0f, 2.0f, 80, 0, 10, new List<Debuff>(), "Does bonus damage if target is poisoned. Removes poison.");
                    characterScript.knownAttacks.Add(newAttack); 
                } 
            }
            else if (sceneName.Contains("Maeve"))
            {
            if (sceneName == "Maeve1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Life Drain", "magical", 1.0f, 1.2f, 90, 0, 4, "Heal for half the damage dealt.");
                    characterScript.knownAttacks.Add(newAttack);
                }

                else if (sceneName == "Maeve2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new SupportMove("Sacrifice", 4, "hp", 5,"Transfer 25% of your health to an ally.");
                    characterScript.knownAttacks.Add(newAttack);  
                }

                else if (sceneName == "Maeve3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Blood Bomb", "magical", 1.0f, 2.0f, 80, 0, 10, "Sacrifice 50% current health to do huge damage.");
                    characterScript.knownAttacks.Add(newAttack); 
                } 
            }   
            else if (sceneName.Contains("Elani"))
            {
            if (sceneName == "Elani1" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Punishment", "physical", 1.5f, 1.0f, 90, 0, 4, "Bonus damage against targets that have a buff.");
                    characterScript.knownAttacks.Add(newAttack);
                }

                else if (sceneName == "Elani2" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Babydoll Eyes", "physical", 1.3f, 1.0f, 75, 0, 6, new List<Debuff>() {new Debuff("Vulnerable", 100, 2)}, "Enemy lowers their gaurd. Set Vulnerable for 2 turns.");
                    characterScript.knownAttacks.Add(newAttack);  
                }

                else if (sceneName == "Elani3" && !characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained)
                {
                    newAttack = new Attack("Climax", "physical", 1.5f, 1.0f, 70, 0, 10, new List<Debuff>(), "The dramatic kind. If target is brought below 20% max HP, target dies.");
                    characterScript.knownAttacks.Add(newAttack); 
                } 
            }   
                    
            if (newAttack != null)
            {
                newAttackBox.SetActive(true);
                newAttackBoxName.text = characterScript.title + " learned a new Attack!";
                newAttackBoxText.text = newAttack.name;
                gainedNewAttackAudio.Play();

                yield return new WaitForSeconds(4f);
                newAttackBox.SetActive(false);

            }
            
            characterSelected.GetComponent<CampPlayerController>().subquests[dialogueIndex].newAttackGained = true;
            active = true;
            yield return null;
        }
        */
    

    }