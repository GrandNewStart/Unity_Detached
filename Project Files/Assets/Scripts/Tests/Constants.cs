using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlIndex { player, firstArm, secondArm, disabled }
public enum State { idle, walk, air, charge, fire }
public enum ArmIndex { first, second }
public enum CrusherState { up, wait, down }
public enum MagnetDirection { up, down, left, right }
public enum MagnetTarget { player, arm, crate, none }

public class Constants
{
    public static float defaultCamSize = 6;
}
