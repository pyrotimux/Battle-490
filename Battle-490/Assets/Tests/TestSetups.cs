using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.Networking;

public class NewPlayModeTest {
    [UnityTest]
    public IEnumerator SetupPlayer() {
        MonoBehaviour.Instantiate(Resources.Load<GameObject>("Player"));
        yield return new WaitForSeconds(1);
    }

    [UnityTest]
    public IEnumerator SetupBlinkingArea() {
        MonoBehaviour.Instantiate(Resources.Load<GameObject>("moveablearea"));
        yield return new WaitForSeconds(1);
    }
}
