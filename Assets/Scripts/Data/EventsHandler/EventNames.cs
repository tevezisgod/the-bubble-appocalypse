using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EventsHandler/" + nameof(EventNames), fileName = nameof(EventNames))]

public class EventNames : ScriptableObject
{
    [Header("Game Events")]
    [SerializeField] private string onLevelStarted;
    [SerializeField] private string onLevelLost;
    [SerializeField] private string onLevelWon;
    [SerializeField] private string onGameWon;

    [Header("Player Events")] 
    [SerializeField] private string onPlayerHit;
    [SerializeField] private string onShotFired;
    [SerializeField] private string onShotHit;

    [Header("Enemy Events")]
    [SerializeField] private string onEnemyKilled;
    [SerializeField] private string onEnemySwarmKilled;
    [SerializeField] private string onEnemyHitWall;

    public string ONLevelLost => onLevelLost;
    public string ONLevelStarted => onLevelStarted;
    public string ONLevelWon => onLevelWon;
    public string ONGameWon => onGameWon;
    public string ONEnemyKilled => onEnemyKilled;
    public string ONPlayerHit => onPlayerHit;
    public string ONEnemySwarmKilled => onEnemySwarmKilled;
    public string ONEnemyHitWall => onEnemyHitWall;
    public string ONShotFired => onShotFired;
    public string ONShotHit => onShotHit;
}
