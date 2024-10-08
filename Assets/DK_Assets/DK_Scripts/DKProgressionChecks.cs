using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoT.AbstractClasses;

public class DKProgressionChecks : MonoSingleton<DKProgressionChecks>
{
    // Main Quest Progression
    public ProgressionCheck[] mainQuestProgression;

    // Nexus Portal Unlock Checks
    public ProgressionCheck[] nexusPortalUnlocks;
}
