using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_STATE { IDLE, MOVE, DASH, JUMP, FALL, BOOST, LAND, ATTACK, DEATH, CAST, GROUNDBREAKER, HURT, TALKING, LAST_NO_USE }
public enum ENEMY_STATE { IDLE, PATROL, CHASE, ATTACK, HIT, DEATH, REST, CHARGE, REPOSITION, PREPARE_ATTACK, RETURN, LAST_NO_USE}
public enum UNITY_LAYERS { DEFAULT, TRANSPARENT_FX, IGNORE_RAYCAST, NO_USE, WATER, UI, ENEMY, PLAYER, OBSTACLE}
// ALFA SCENE
public enum ROOMS { FOREST_1, FOREST_2, FIRST_CHAMBER, SECRET_1, PRE_FIRE, FIRE, SHAFT, TREE_TRUNK, HALLWAY_1, TREE_1, TREE_2, TREE_3, HALLWAY_2, LAST_NO_USE }

public enum ANIMATION {PLAYER_IDLE, PLAYER_RUN, PLAYER_DASH, PLAYER_JUMP, PLAYER_FALL, PLAYER_BOOST, PLAYER_LAND, PLAYER_HIT, PLAYER_GROUNDBREAKER, PLAYER_GROUNDBREAKER_LOOP, PLAYER_DEATH,
GHOUL_IDLE, GHOUL_MOVE, GHOUL_ATTACK, GHOUL_HIT, GHOUL_DIE, GHOUL_CHARGE,
CHARGE_BAT_MOVE, CHARGE_BAT_PREPARE_ATTACK, CHARGE_BAT_ATTACK, CHARGE_BAT_RECOVER_FROM_ATTACK, CHARGE_BAT_HIT, CHARGE_BAT_DIE,
SKELETON_MOVE, SKELETON_RELOAD, SKELETON_FIRE, SKELETON_HIT, SKELETON_ATTACK, SKELETON_DIE,
PATROL_BAT_MOVE, PATROL_BAT_HIT, PATROL_BAT_DIE,
SPIDER_BOSS_IDLE, SPIDER_BOSS_ROAR, SPIDER_BOSS_ROAR_LOOP, SPIDER_BOSS_MOVE_LEFT, SPIDER_BOSS_MOVE_RIGHT, SPIDER_BOSS_ATTACK_LEFT, SPIDER_BOSS_ATTACK_RIGHT, SPIDER_BOSS_RECOVER_TERRAIN_LEFT, SPIDER_BOSS_RECOVER_TERRAIN_RIGHT, SPIDER_BOSS_RECOVER_NORMAL_LEFT, SPIDER_BOSS_RECOVER_NORMAL_RIGHT, SPIDER_BOSS_ATTACK_BITE, SPIDER_BOSS_ATTACK_SPIT,
 LAST_NO_USE}

public enum SKILLS { DASH, FIRE_BALL, GROUNDBREAKER, PILAR, LAST_NO_USE}

public enum BACKGROUND_CLIP { BACKGROUND_1, LAST_NO_USE}
 public enum SCENE { MAIN_MENU, GAME }
 enum ACTIONS { ATTACK, DASH, JUMP, SKILL_1, SKILL_2, SKILL_3, INTERACT, CANCEL, UP_UI, BOTTOM_UI, NUMBER_OF_ACTIONS };
 enum SPIDER_BOSS_STATE { IDLE, CHASE, ATTACK_DRILL, ATTACK_BITE, STUCK_TERRAIN, RECOVER_TERRAIN, RECOVER_NORMAL, LAST_NO_USE}