public static class Player {

  /*
   * define named IDs for settings as integer constants
   */

  // ATTENTION: IDs in this block must be the same as defined in Level.cs 
  // [ 
  public const int CAN_MOVE = 1;
    public const int CAN_JUMP = 2;
    public const int CAN_SELF_DESTRUCT = 3;
    public const int CAN_MORPH_TO_CIRCLE = 4;
    public const int CAN_MORPH_TO_TRIANGLE = 5;
    public const int CAN_MORPH_TO_RECTANGLE = 6;
  // ]

  public const int HAS_SPAWN_POINT = 7;
  public const int STEPPED_ON_PISTON = 8;
  public const int HOLDING_ITEM = 9;
  public const int IS_DEAD = 10;
  public const int SECONDS_NOT_GROUNDED = 11;
  public const int SECONDS_AS_RECTANGLE_FALLING = 12;
  public const int STATE = 13;


  public static class Sprites {

    /*
     * constants defining sprite arrays ID
     */

    public const int TRIANGLE_TO_CIRCLE = 101;
    public const int RECTANGLE_TO_CIRCLE = 102;
    public const int RECTANGLE_TO_TRIANGLE = 103;

  }

  public static class Objects {

    /*
     * constants defining objects used by player with IDs
     */

    public const int PARENT_OBJECT = 201;
    public const int TEXTURE_OBJECT = 202;
    public const int HELD_ITEM_OBJECT = 203;
    public const int TEXTURE_CONTAINER = 204;
    public const int MOVEMENT_PARTICLES = 205;
    public const int DEATH_PARTICLES = 206;
    public const int DOUBLE_JUMP_PARTICLES = 207;

  }

}
