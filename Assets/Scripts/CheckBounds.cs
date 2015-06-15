using System.Collections;
using UnityEngine;

public class CheckBounds
{
    // Use this for class variables.

    public Vector2 gameBounds;
    public Vector2 screenBounds;

    public CheckBounds(Camera camera, Transform distanceFromCamera, float gameWorldMultiplyer)
    {
        float distance = distanceFromCamera.position.z * -1;
        float frustumHeight = 2 * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = (frustumHeight * camera.aspect);

        screenBounds.x = frustumWidth * 0.5f;
        screenBounds.y = frustumHeight * 0.5f;

        gameBounds = screenBounds * gameWorldMultiplyer;
    }

    public Vector3 CheckOffWorld(Vector3 currentLocation)
    {
        Vector3 newLocation = currentLocation;

        if (currentLocation.x > gameBounds.x)
            newLocation = new Vector3(gameBounds.x, currentLocation.y, currentLocation.z);

        if (currentLocation.x < -gameBounds.x)
            newLocation = new Vector3(-gameBounds.x, currentLocation.y, currentLocation.z);

        if (currentLocation.y > gameBounds.y)
            newLocation = new Vector3(currentLocation.x, gameBounds.y, currentLocation.z);

        if (currentLocation.y < -gameBounds.y)
            newLocation = new Vector3(currentLocation.x, -gameBounds.y, currentLocation.z);

        return newLocation;
    }

    public Vector3 CheckOffScreen(Vector3 currentLocation)
    {
        Vector3 newLocation = currentLocation;

        if (currentLocation.x > gameBounds.x)
            newLocation = new Vector3(screenBounds.x, currentLocation.y, currentLocation.z);

        if (currentLocation.x < -gameBounds.x)
            newLocation = new Vector3(-screenBounds.x, currentLocation.y, currentLocation.z);

        if (currentLocation.y > gameBounds.y)
            newLocation = new Vector3(currentLocation.x, screenBounds.y, currentLocation.z);

        if (currentLocation.y < -gameBounds.y)
            newLocation = new Vector3(currentLocation.x, -screenBounds.y, currentLocation.z);

        return newLocation;
    }

    public Vector3 BounceOffEdge(Vector3 currentLocation, Vector3 currentVelocity)
    {
        Vector3 newVelocity = currentVelocity;

        if (currentLocation.x > gameBounds.x)
            newVelocity = new Vector3(currentVelocity.x * -1, currentVelocity.y, currentVelocity.z);

        if (currentLocation.x < -gameBounds.x)
            newVelocity = new Vector3(currentVelocity.x * -1, currentVelocity.y, currentVelocity.z);

        if (currentLocation.y > gameBounds.y)
            newVelocity = new Vector3(currentVelocity.x, currentVelocity.x * -1, currentVelocity.z);

        if (currentLocation.y < -gameBounds.y)
            newVelocity = new Vector3(currentVelocity.x, currentVelocity.x * -1, currentVelocity.z);

        return newVelocity;
    }
}