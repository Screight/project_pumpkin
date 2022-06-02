using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_STATE { IDLE, MOVE, DASH, JUMP, FALL, BOOST, LAND, ATTACK, DEATH, CAST, GROUNDBREAKER, HURT, TALKING, LAST_NO_USE }
public enum ENEMY_STATE { IDLE, PATROL, CHASE, ATTACK, HIT, DEATH, REST, CHARGE, REPOSITION, PREPARE_ATTACK, RETURN, EGG, ECLOSION, LAST_NO_USE}
public enum UNITY_LAYERS { DEFAULT, TRANSPARENT_FX, IGNORE_RAYCAST, NO_USE, WATER, UI, ENEMY, PLAYER, OBSTACLE}
// ALFA SCENE
public enum ROOMS { FOREST_1, FOREST_2, FIRST_CHAMBER, SECRET_1, PRE_FIRE, FIRE, SHAFT, TREE_TRUNK, HALLWAY_1, TREE_1, TREE_2, TREE_3, HALLWAY_2, LAST_NO_USE }

public enum ANIMATION {PLAYER_IDLE, PLAYER_RUN, PLAYER_DASH, PLAYER_JUMP, PLAYER_FALL, PLAYER_BOOST, PLAYER_LAND, PLAYER_HIT, PLAYER_GROUNDBREAKER, PLAYER_GROUNDBREAKER_LOOP, PLAYER_DEATH,
GHOUL_IDLE, GHOUL_MOVE, GHOUL_ATTACK, GHOUL_HIT, GHOUL_DIE, GHOUL_CHARGE,
CHARGE_BAT_MOVE, CHARGE_BAT_PREPARE_ATTACK, CHARGE_BAT_ATTACK, CHARGE_BAT_RECOVER_FROM_ATTACK, CHARGE_BAT_HIT, CHARGE_BAT_DIE,
SKELETON_MOVE, SKELETON_RELOAD, SKELETON_FIRE, SKELETON_HIT, SKELETON_ATTACK, SKELETON_DIE,
PATROL_BAT_MOVE, PATROL_BAT_HIT, PATROL_BAT_DIE,
SPIDER_BOSS_IDLE, SPIDER_BOSS_ROAR, SPIDER_BOSS_ROAR_LOOP, SPIDER_BOSS_MOVE_LEFT, SPIDER_BOSS_MOVE_RIGHT, SPIDER_BOSS_ATTACK_LEFT, SPIDER_BOSS_ATTACK_RIGHT, SPIDER_BOSS_RECOVER_TERRAIN_LEFT, SPIDER_BOSS_RECOVER_TERRAIN_RIGHT, SPIDER_BOSS_RECOVER_NORMAL_LEFT, SPIDER_BOSS_RECOVER_NORMAL_RIGHT, SPIDER_BOSS_ATTACK_BITE, SPIDER_BOSS_ATTACK_SPIT, SPIDER_BOSS_MOVE, SPIDER_BOSS_LOSE_LEFT_LEG, SPIDER_BOSS_LOSE_RIGHT_LEG,
SPIDER_BOSS_LEFT_LEG_EXPLOSION,
SPIDER_BOSS_RIGHT_LEG_EXPLOSION,
SPIDER_WALK, SPIDER_EGG, SPIDER_ECLOSION,
EFFECT_ACID_LOOP, EFFECT_ACID_EXPLOSION,
PORTAL_OPEN, PORTAL_CLOSE, PORTAL_IDLE, LIFE_URA_UP, LIFE_URA_DOWN,
 LAST_NO_USE}

public enum SKILLS { DASH, FIRE_BALL, GROUNDBREAKER, PILAR, LAST_NO_USE }

public enum BACKGROUND_CLIP { MAINTITLE, FORESTOFSOULS, ABANDONEDMINE, SPIDER_BOSS, LAST_NO_USE }
public enum SCENE { MAIN_MENU, GAME }
public enum ACTIONS { ATTACK, DASH, JUMP, SKILL_1, SKILL_2, SKILL_3, INTERACT, CANCEL, UP_UI, BOTTOM_UI, PAUSE, MAP, NUMBER_OF_ACTIONS };
public enum SPIDER_BOSS_STATE
{
    IDLE, CHASE, ATTACK_DRILL, ATTACK_BITE, ATTACK_ACID, STUCK_TERRAIN, RECOVER_TERRAIN, RECOVER_NORMAL, LOSE_LEG, EGG_SPAWN, RETURN_TO_CENTER,
    SPAWN_EGGS, LAST_NO_USE
}
public enum SPIDER_BOSS_DAMAGEABLE_PARTS { LEFT_DRILL, RIGHT_DRILL, HEAD, LAT_NO_USE}
public enum ZONE { FOREST, MINE, LAST_NO_USE }
public enum AudioClipName
{
    ENEMY_HIT, ENEMY_KILL, FIREBALL, PILAR, SKELLY_SHOOT,
    PLAYER_ATTACK_1, PLAYER_ATTACK_2, PLAYER_ATTACK_3,
    LOW_HP, GROUNDBREAKER, ITEM_PICK_UP, DASH,
    EGG_CRACK_1, EGG_CRACK_2,
    SPIDER_BOSS_LOSE_LEG, SPIDER_BOSS_CRY, SPIDER_BOSS_CRY_LITE,
    DRILL_HIT_1, DRILL_HIT_2, DRILL_ATTACK, DRILL_STUCKED,
    URA_HIT_1, URA_HIT_2, URA_HIT_3, URA_HIT_4, JUMP, RESPAWN,
    DIALOGUECLIC1, DIALOGUECLIC2, BUTTONSWITCH, BUTTONCLICKED,
    PORTALUSE, PORTALOPEN, PORTALLOOP, PORTALCLOSE, RUNESFX, SAVESFX,
    SPIDER_HISS_1, SPIDER_HISS_2,
    GHOUL_NOISE_1, GHOUL_NOISE_2, GHOUL_NOISE_3,
    GHOUL_ATK_1, GHOUL_ATK_2, GHOUL_ATK_3,
    SKELLY_GROWL_1, SKELLY_GROWL_2,
    SPIRIT_TRAIL,
    LILBAT_SWING_1, LILBAT_SWING_2, LILBAT_SWING_3,
    LAST_NO_USE
}