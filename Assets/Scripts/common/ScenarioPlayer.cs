using System;
using UnityEngine;
using UnityEngine.UI;

namespace Komugi.UI
{
    public class ScenarioPlayer : MonoBehaviour
    {
        private static string PREFAB_PATH = "Prefabs/ui/scenario_player";

        public string[] Scenario { get; set; }

        public Color TextColor { get; set; }

        public Action CallBack = null;

        private Text scenarioText;

        private int count = 0;
        private int line = 0;
        private string completeText = string.Empty;

        // Use this for initialization
        void Start()
        {
            scenarioText = GetComponentInChildren<Text>();
            count = 0;
            line = 0;
            scenarioText.color = TextColor;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (line < Scenario.Length)
            {
                string nowText = Scenario[line];
                count++;
                int index = count / 6;

                if (index <= nowText.Length)
                {
                    scenarioText.text = completeText + nowText.Substring(0, index);
                }
                else
                {
                    scenarioText.text = scenarioText.text + "\n";
                    line++;
                    count = 0;
                    completeText = scenarioText.text;
                }
            }
            else
            {
                enabled = false;
                if (CallBack != null)
                {
                    CallBack();
                }
            }
        }

        public static void PlayScenario(string[] scenario, Transform content, Color color, Action callBack = null)
        {
            GameObject obj = Resources.Load(PREFAB_PATH, typeof(GameObject)) as GameObject;
            GameObject splayer = Instantiate(obj) as GameObject;

            ScenarioPlayer sp = splayer.GetComponent<ScenarioPlayer>();
            if (sp != null)
            {
                sp.Scenario = scenario;
                splayer.transform.SetParent(content, false);
                sp.CallBack = callBack;
                sp.TextColor = color;
            }
        }
    }
}