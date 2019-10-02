using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

[RequireComponent(typeof(Ship))]
public class ShipController : MonoBehaviour, ITargetController
{
    private Queue<ICommand> commands;
    private Ship ship;

    public Ship Ship { get => ship; }

    private void Awake()
    {
        ship = GetComponent<Ship>();
        commands = new Queue<ICommand>();
    }

    public List<WeaponsSys.Weapon> GetWeapons()
    {
        return ship.weapons.GetWeapons();
    }

    public float GetSpeed()
    {
        return ship.Speed;
    }

    public float GetRudderAngle()
    {
        return ship.rudderAngleAdjustment;
    }

    public void AddCommand(ICommand command)
    {
        commands.Enqueue(command);
    }

    private void Update()
    {
        if (commands.Count > 0 && !ship.CrashRecovering)
            commands.Dequeue().Execute();
    }

    public void Stop()
    {
        AddCommand(new ShipCommands.Move(ship, Ship.EOT.Stop));
    }

    public void FullAhead()
    {
        AddCommand(new ShipCommands.Move(ship, Ship.EOT.FullAhead));
    }

    public void HalfAhead()
    {
        AddCommand(new ShipCommands.Move(ship, Ship.EOT.HalfAhead));
    }

    public void FullAstern()
    {
        AddCommand(new ShipCommands.Move(ship, Ship.EOT.FullAstern));
    }

    public void HalfAstern()
    {
        AddCommand(new ShipCommands.Move(ship, Ship.EOT.HalfAstern));
    }

    public void SlowAstern()
    {
        AddCommand(new ShipCommands.Move(ship, Ship.EOT.SlowAstern));
    }

    public void SlowAhead()
    {
        AddCommand(new ShipCommands.Move(ship, Ship.EOT.SlowAhead));
    }

    public void TurnPort()
    {
        AddCommand(new ShipCommands.Steer(ship, ShipCommands.Steer.Direction.Port));
    }

    public void TurnStarboard()
    {
        AddCommand(new ShipCommands.Steer(ship, ShipCommands.Steer.Direction.Starboard));
    }

    public void TurnAhead()
    {
        AddCommand(new ShipCommands.Steer(ship, ShipCommands.Steer.Direction.Ahead));
    }

    public void AddOnCrashAction(UnityAction action)
    {
        ship.OnCrash += action;
    }

    public void AddOnDestroyAction(UnityAction action)
    {
        ship.OnDestroy += action;
    }

    public void RemoveOnCrashAction(UnityAction action)
    {
        ship.OnCrash -= action;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3 GetHeading()
    {
        return ship.Heading;
    }
}
