using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarTrigger : MonoBehaviour
{
    private void Update()
    {
        SimpleSonarShader_Parent parent = GetComponentInParent<SimpleSonarShader_Parent>();
        if (parent) parent.StartSonarRing(transform.position, 10);
    }
}
