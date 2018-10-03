using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using messyWires;

public class messyWiresScript : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public GameObject[] wholeWires;
    public GameObject[] cutWires;
    public Renderer[] wholeWiresRend;
    public Renderer[] cutWiresRend;
    public Material[] wireMaterials;
    public bool[] wireIsActive;
    public KMSelectable[] wireSelectables;
    public string[] colourOptions;
    public string[] wireColourName;

    private int totalWiresActive = 0;
    public int[] colourTotals;

    public string[] cuttingOrder;
    private string tempColour1;
    private string tempColour2;

    private int greyBlackWhite = 0;
    private int aquaWires = 0;
    private int portPlates = 0;
    private int yellowRedOrange = 0;
    private int limePink = 0;
    private int greenPurple = 0;
    private int redPinkAqua = 0;
    private int litIndicators = 0;
    private int blackBrown = 0;
    private int greenDarkGrey = 0;
    private int batteries = 0;
    private int yellowOrange = 0;
    private int greyLime = 0;

    public List<String> orderOfActiveWires = new List<string>();
    private int numberOfCorrectCuts = 0;
    private int totalCuts = 0;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;

    void Awake()
    {
        moduleId = moduleIdCounter++;
        foreach (KMSelectable wire in wireSelectables)
        {
            KMSelectable cutWire = wire;
            wire.OnInteract += delegate () { OnWireCut(cutWire); return false; };
        }
    }

    void Start()
    {
        SelectWiresAndColours();
    }

    void SelectWiresAndColours()
    {
        for(int i = 0; i <= 20; i++)
        {
            cutWires[i].SetActive(false);
            wholeWires[i].SetActive(false);
        }

        wholeWires[0].SetActive(true);
        int wireColourIndexAlways = UnityEngine.Random.Range(0,15);
        colourTotals[wireColourIndexAlways]++;
        wholeWiresRend[0].material = wireMaterials[wireColourIndexAlways];
        cutWiresRend[0].material = wireMaterials[wireColourIndexAlways];
        wireColourName[0] = colourOptions[wireColourIndexAlways];
        wireIsActive[0] = true;
        totalWiresActive++;

        for(int i = 1; i <= 20; i++)
        {
            int wireActive = UnityEngine.Random.Range(0,3);
            if(wireActive == 0 || wireActive == 1)
            {
                wholeWires[i].SetActive(true);
                int wireColourIndex = UnityEngine.Random.Range(0,15);
                colourTotals[wireColourIndex]++;
                wholeWiresRend[i].material = wireMaterials[wireColourIndex];
                cutWiresRend[i].material = wireMaterials[wireColourIndex];
                wireColourName[i] = colourOptions[wireColourIndex];
                wireIsActive[i] = true;
                totalWiresActive++;
            }
            else
            {
                wholeWiresRend[i].material = wireMaterials[15];
                cutWiresRend[i].material = wireMaterials[15];
                wireColourName[i] = "";
                wireIsActive[i] = false;
            }
        }
        Debug.LogFormat("[Wire Spaghetti #{0}] There are {1} wires:", moduleId, totalWiresActive);
        for(int i = 0; i <= colourTotals.Count() - 1; i++)
        {
            if(colourTotals[i] == 1)
            {
                Debug.LogFormat("[Wire Spaghetti #{0}] {1}x {2}", moduleId, colourTotals[i], colourOptions[i]);
            }
            else if(colourTotals[i] > 1)
            {
                Debug.LogFormat("[Wire Spaghetti #{0}] {1}x {2}", moduleId, colourTotals[i], colourOptions[i]);
            }
        }
        DetermineCuttingOrder();
    }

    void DetermineCuttingOrder()
    {
        greyBlackWhite = colourTotals[1] + colourTotals[4] + colourTotals[7] + colourTotals[13];
        if(greyBlackWhite < 5)
        {
            tempColour1 = cuttingOrder[0];
            tempColour2 = cuttingOrder[6];
            cuttingOrder[0] = tempColour2;
            cuttingOrder[6] = tempColour1;
            Debug.LogFormat("[Wire Spaghetti #{0}] The total number of black, white, dark grey & light grey wires is {1}. The first statement is true.", moduleId, greyBlackWhite);
        }
        else
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] The total number of black, white, dark grey & light grey wires is {1}. The first statement is false.", moduleId, greyBlackWhite);
        }

        aquaWires = colourTotals[0];
        if(aquaWires == 0)
        {
            tempColour1 = cuttingOrder[2];
            tempColour2 = cuttingOrder[5];
            cuttingOrder[2] = tempColour2;
            cuttingOrder[5] = tempColour1;
            Debug.LogFormat("[Wire Spaghetti #{0}] There are no aqua wires. The second statement is true.", moduleId);
        }
        else
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] There are {1} aqua wires. The second statement is false.", moduleId, aquaWires);
        }

        portPlates = Bomb.GetPortPlates().Count();
        yellowRedOrange = colourTotals[14] + colourTotals[10] + colourTotals[5];
        if(portPlates > yellowRedOrange)
        {
            tempColour1 = cuttingOrder[4];
            tempColour2 = cuttingOrder[13];
            cuttingOrder[4] = tempColour2;
            cuttingOrder[13] = tempColour1;
            Debug.LogFormat("[Wire Spaghetti #{0}] The number of port plates is {1}. The total number of yellow, dark red & orange wires is {2}. The third statement is true.", moduleId, portPlates, yellowRedOrange);
        }
        else
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] The number of port plates is {1}. The total number of yellow, dark red & orange wires is {2}. The third statement is false.", moduleId, portPlates, yellowRedOrange);
        }

        limePink = colourTotals[9] + colourTotals[11];
        greenPurple = colourTotals[6] + colourTotals[12];
        if(limePink > greenPurple)
        {
            tempColour1 = cuttingOrder[6];
            tempColour2 = cuttingOrder[10];
            cuttingOrder[6] = tempColour2;
            cuttingOrder[10] = tempColour1;
            Debug.LogFormat("[Wire Spaghetti #{0}] The total number of lime & pink wires is {1}. The total number of green & purple wires is {2}. The fourth statement is true.", moduleId, limePink, greenPurple);
        }
        else
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] The total number of lime & pink wires is {1}. The total number of green & purple wires is {2}. The fourth statement is false.", moduleId, limePink, greenPurple);
        }

        if((colourTotals[8] < colourTotals[3]) && (colourTotals[1] > colourTotals[2]))
        {
            tempColour1 = cuttingOrder[13];
            tempColour2 = cuttingOrder[14];
            cuttingOrder[13] = tempColour2;
            cuttingOrder[14] = tempColour1;
            Debug.LogFormat("[Wire Spaghetti #{0}] The number of light red wires is {1}. The number of brown wires is {2}. The number of black wires is {3}. The number of blue wires is {4}. The fifth statement is true.", moduleId, colourTotals[8], colourTotals[3], colourTotals[1], colourTotals[2]);
        }
        else
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] The number of light red wires is {1}. The number of brown wires is {2}. The number of black wires is {3}. The number of blue wires is {4}. The fifth statement is false.", moduleId, colourTotals[8], colourTotals[3], colourTotals[1], colourTotals[2]);
        }

        if(totalWiresActive > 13)
        {
            tempColour1 = cuttingOrder[1];
            tempColour2 = cuttingOrder[12];
            cuttingOrder[1] = tempColour2;
            cuttingOrder[12] = tempColour1;
            Debug.LogFormat("[Wire Spaghetti #{0}] The total number of wires is {1}. The sixth statement is true.", moduleId, totalWiresActive);
        }
        else
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] The total number of wires is {1}. The sixth statement is false.", moduleId, totalWiresActive);
        }

        redPinkAqua = colourTotals[5] + colourTotals[11] + colourTotals[0];
        if(redPinkAqua == 2 || redPinkAqua == 3 || redPinkAqua == 5 || redPinkAqua == 7 || redPinkAqua == 11 || redPinkAqua == 13 || redPinkAqua == 17 || redPinkAqua == 19)
        {
            tempColour1 = cuttingOrder[4];
            tempColour2 = cuttingOrder[9];
            cuttingOrder[4] = tempColour2;
            cuttingOrder[9] = tempColour1;
            Debug.LogFormat("[Wire Spaghetti #{0}] The total number of dark red, pink & aqua wires is {1}. The seventh statement is true.", moduleId, redPinkAqua);
        }
        else
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] The total number of dark red, pink & aqua wires is {1}. The seventh statement is false.", moduleId, redPinkAqua);
        }

        litIndicators = Bomb.GetOnIndicators().Count();
        blackBrown = colourTotals[1] + colourTotals[3];
        if(litIndicators < blackBrown)
        {
            tempColour1 = cuttingOrder[7];
            tempColour2 = cuttingOrder[10];
            cuttingOrder[7] = tempColour2;
            cuttingOrder[10] = tempColour1;
            Debug.LogFormat("[Wire Spaghetti #{0}] The number of lit indicators is {1}. The total number of black & brown wires is {2}. The eighth statement is true.", moduleId, litIndicators, blackBrown);
        }
        else
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] The number of lit indicators is {1}. The total number of black & brown wires is {2}. The eighth statement is false.", moduleId, litIndicators, blackBrown);
        }

        greenDarkGrey = colourTotals[6] + colourTotals[4];
        if(greenDarkGrey > (colourTotals[11] * 2))
        {
            tempColour1 = cuttingOrder[2];
            tempColour2 = cuttingOrder[7];
            cuttingOrder[2] = tempColour2;
            cuttingOrder[7] = tempColour1;
            Debug.LogFormat("[Wire Spaghetti #{0}] The total number of green and dark grey wires is {1}. Twice the number of pink wires is {2}. The ninth statement is true.", moduleId, greenDarkGrey, colourTotals[11] * 2);
        }
        else
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] The total number of green and dark grey wires is {1}. Twice the number of pink wires is {2}. The ninth statement is false.", moduleId, greenDarkGrey, colourTotals[11] * 2);
        }

        if(colourTotals[13] >= 3)
        {
            tempColour1 = cuttingOrder[8];
            tempColour2 = cuttingOrder[11];
            cuttingOrder[8] = tempColour2;
            cuttingOrder[11] = tempColour1;
            Debug.LogFormat("[Wire Spaghetti #{0}] The number of white wires is {1}. The tenth statement is true.", moduleId, colourTotals[13]);
        }
        else
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] The number of white wires is {1}. The tenth statement is false.", moduleId, colourTotals[13]);
        }

        batteries = Bomb.GetBatteryCount();
        yellowOrange = colourTotals[14] + colourTotals[10];
        if(batteries == yellowOrange)
        {
            tempColour1 = cuttingOrder[3];
            tempColour2 = cuttingOrder[13];
            cuttingOrder[3] = tempColour2;
            cuttingOrder[13] = tempColour1;
            Debug.LogFormat("[Wire Spaghetti #{0}] The number of batteries is {1}. The total number of yellow & orange wires is {2}. The eleventh statement is true.", moduleId, batteries, yellowOrange);
        }
        else
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] The number of batteries is {1}. The total number of yellow & orange wires is {2}. The eleventh statement is false.", moduleId, batteries, yellowOrange);
        }

        greyLime = colourTotals[7] + colourTotals[9];
        if(greyLime == colourTotals[3])
        {
            tempColour1 = cuttingOrder[0];
            tempColour2 = cuttingOrder[1];
            cuttingOrder[0] = tempColour2;
            cuttingOrder[1] = tempColour1;
            Debug.LogFormat("[Wire Spaghetti #{0}] The number of brown wires is {1}. The total number of light grey & lime wires is {2}. The twelfth statement is true.", moduleId, colourTotals[3], greyLime);
        }
        else
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] The number of brown wires is {1}. The total number of light grey & lime wires is {2}. The twelfth statement is false.", moduleId, colourTotals[3], greyLime);
        }
        orderOfActiveWires = wireColourName.OrderBy(x => Array.IndexOf(cuttingOrder, x)).Where(x => x != "").ToList();
        Debug.LogFormat("[Wire Spaghetti #{0}] Cut the wires in this order: {1}.", moduleId, string.Join(", ", orderOfActiveWires.Select((x) => x).ToArray()));
    }

    public void OnWireCut(KMSelectable wire)
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.WireSnip, wire.transform);
        wire.AddInteractionPunch();
        wire.gameObject.SetActive(false);
        cutWires[Array.IndexOf(wholeWires, wire.gameObject)].SetActive(true);
        string cutColour = wireColourName[Array.IndexOf(wholeWires, wire.gameObject)];
        if(cutColour == orderOfActiveWires[numberOfCorrectCuts])
        {
            Debug.LogFormat("[Wire Spaghetti #{0}] The wire you cut was {1}. That is correct.", moduleId, cutColour);
            numberOfCorrectCuts++;
        }
        else
        {
            GetComponent<KMBombModule>().HandleStrike();
            Debug.LogFormat("[Wire Spaghetti #{0}] Strike! The wire you cut was {1}. That is incorrect. I was expecting {2}.", moduleId, cutColour, orderOfActiveWires[numberOfCorrectCuts]);
            orderOfActiveWires.Remove(cutColour);
        }
        totalCuts++;
        if(totalCuts == totalWiresActive)
        {
            GetComponent<KMBombModule>().HandlePass();
            Debug.LogFormat("[Wire Spaghetti #{0}] Module disarmed.", moduleId);
        }
    }

    private string TwitchHelpMessage = @"Use '!{0} cut p l l dr dr dr' to cut the wires in that order. Valid colors are Purple, Lime, DarkRed, White, Green, Orange, Blue, Yellow, LightRed, blacK, DarkGrey, pInk, Aqua, bRown, and LightGrey.";

    IEnumerator ProcessTwitchCommand(string command)
    {
        var parts = command.ToLowerInvariant().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length > 1 && parts[0] == "cut" && parts.Skip(1).All(part => (part.Length == 1 && ("plwgobykiar".Contains(part)) || part.Length == 2 && new[] { "dr", "lr", "dg", "lg" }.Any(ix => part == ix))))
        {
            yield return null;

            for (int i = 1; i < parts.Length; i++)
            {
                string colorName = ConvertToColor(parts[i]);

                if (!wireSelectables.Any(w => colorName == wireColourName[Array.IndexOf(wholeWires, w.gameObject)]))
                {
                    yield return "sendtochaterror The color, " + colorName + ", isn't on the module!";
                    yield return "unsubmittablepenalty";
                    yield break;
                }

                foreach (KMSelectable wire in wireSelectables.Where(w => colorName == wireColourName[Array.IndexOf(wholeWires, w.gameObject)]))
                {
                    if (wire.gameObject.activeInHierarchy == true) // If the wire isn't cut
                    {
                        OnWireCut(wire);
                        yield return new WaitForSeconds(.1f);
                        break;
                    }
                }
            }
        }
    }

    private string ConvertToColor(string color)
    {
        string conversion;

        if (color == "p") { conversion = "purple"; }
        else if (color == "l") { conversion = "lime"; }
        else if (color == "dr") { conversion = "dark red"; }
        else if (color == "w") { conversion = "white"; }
        else if (color == "g") { conversion = "green"; }
        else if (color == "o") { conversion = "orange"; }
        else if (color == "b") { conversion = "blue"; }
        else if (color == "y") { conversion = "yellow"; }
        else if (color == "lr") { conversion = "light red"; }
        else if (color == "k") { conversion = "black"; }
        else if (color == "dg") { conversion = "dark grey"; }
        else if (color == "i") { conversion = "pink"; }
        else if (color == "a") { conversion = "aqua"; }
        else if (color == "r") { conversion = "brown"; }
        else { conversion = "light grey"; }

        return conversion;
    }
}
