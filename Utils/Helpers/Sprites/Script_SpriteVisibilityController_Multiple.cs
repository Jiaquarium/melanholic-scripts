using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fade Points should describe ranges, excluding the last range
/// </summary>
public class Script_SpriteVisibilityController_Multiple : Script_SpriteVisibilityController
{
    [SerializeField] private Script_FadePoint[] fadePoints;
    [SerializeField] private int fadePointIdx;
    [SerializeField] private int lastFadePointIdx;
    
    protected override bool CheckShouldFadeIn()
    {
        // Find which Fade Range we are in
        lastFadePointIdx = fadePointIdx;
        fadePointIdx = FindFadeRange();
        
        Model_FadePoint fadePointData = fadePoints[fadePointIdx].data;
        Model_FadePoint lastFadePointData = fadePoints[lastFadePointIdx].data;
        
        /// If coming back from a higher Fade Point use the higher Fade Point Node
        fadeSpeed = lastFadePointIdx > fadePointIdx
            ? lastFadePointData.fadeSpeed
            : fadePointData.fadeSpeed;
        
        return fadePointData.state == Model_FadePoint.States.Show;
    }

    private int FindFadeRange()
    {
        Vector3 playerLoc = g.GetPlayerLocation();

        for (int i = 0; i < fadePoints.Length - 1; i++)
        {
            bool isActiveRange;
            
            float zStart  = fadePoints[i].data.target.position.z;
            float zEnd    = fadePoints[i + 1].data.target.position.z;

            float xStart  = fadePoints[i].data.target.position.x;
            float xEnd    = fadePoints[i + 1].data.target.position.x;

            if (isAxisZ)    isActiveRange = playerLoc.z >= zStart && playerLoc.z < zEnd;
            else            isActiveRange = playerLoc.x >= xStart && playerLoc.x < xEnd;

            if (isActiveRange)  return i;
        }

        Debug.LogError("Player is not in any defined Fade Ranges");
        return -1;
    }
}
