using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace DotAdventure
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 800;
            this.graphics.PreferredBackBufferHeight = 600;

            //refresh rate
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = true;


            //load config file
            System.IO.StreamReader settings = new System.IO.StreamReader("settings.cfg");

            string conversion_string = settings.ReadLine();

            if (conversion_string == "fullscreen")
            {
                this.graphics.IsFullScreen = true;
            }
            else
            {
                this.graphics.IsFullScreen = false;
            }

            conversion_string = settings.ReadLine();

            if (conversion_string == "debug")
            {
                debugOn = true;
            }

            settings.Close();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here





            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        /// 

        Texture2D playerSprite;

        Texture2D mrstickSprite;
        Rectangle[] mrstickFrames = new Rectangle[48];
        int mrstickFrameNumber = 0;
        short showLeftOrRight = 0;
        int mrstickAnimationSpeed = 0;  //for limiting the speed at which the player sprite is animated
        bool playerFacingRight = true;
        string animationState = "none";

        Vector2 playerPos = new Vector2(256, 256);
        Rectangle playerRect = new Rectangle();


        //hud
        Texture2D healthMeter;
        Rectangle[] healthMeterRect = new Rectangle[3];
        int score = 0;

        //intro variables
        int introTimer = 0;
        int endingTimer = 0;


        Texture2D blackBorder;
        Texture2D introLightning;
        Texture2D introBall;
        short introBallFrameNumber = 0;
        short introBallAnimationSpeed = 0;
        Rectangle[] introBallRect = new Rectangle[4];
        Vector2 introBallPos = new Vector2(6580, 1650);
        bool introBallGoingRight = true;
        short kickAnimationSpeed = 0;
        Texture2D kickRed;
        Texture2D kickBlue;
        short kickRedFrameNumber = 0;
        short kickBlueFrameNumber = 0;
        //Vector2 kickRedPos = new Vector2(6454, 1614);
        //Vector2 kickBluePos = new Vector2(7134, 1614);
        Vector2 kickRedPos = new Vector2(6480, 1640);
        Vector2 kickBluePos = new Vector2(7080, 1640);
        Rectangle[] kickRedRect = new Rectangle[8];
        Rectangle[] kickBlueRect = new Rectangle[8];
        Texture2D introText0;
        Texture2D introText1;
        Texture2D introText2;
        Texture2D introText3;
        Texture2D introText4;
        Texture2D introText5;
        Texture2D introText6;
        Texture2D introText7;
        Texture2D introText8;
        Texture2D introText9;
        Texture2D introText10;

        //for the games menu's
        string menuShowing = "main";
        string optionSelected = "startGame";
        Texture2D menuMain;
        Texture2D menuOptions;
        Texture2D menuPaused;
        Texture2D menuPointer;
        Texture2D menuSlider;
        Texture2D menuSlider2;
        Texture2D menuExit;
        //Rectangle sfxSliderRect = new Rectangle(0, 0, 10, 35);
        //Rectangle musicSliderRect = new Rectangle(0, 0, 10, 35);
        //Rectangle menuPointerrect = new Rectangle(0, 0, 33, 33);
        //Vector2 sfxSliderPos = new Vector2(0, 0);
        //Vector2 musicSliderPos = new Vector2(0, 0);
        Vector2 menuPointerPos = new Vector2(284, 474);
        Vector2 menuSlider1Pos = new Vector2(542, 472);//Vector2 menuSlider1Pos = new Vector2(552, 472);
        Vector2 menuSlider2Pos = new Vector2(612, 514);
        

        float musicVol = 0.3f;
        float sfxVol = 1.0f;

        

        float gravity = 0.0f;
        bool canJump = false;
        bool jumping = false;
        float moveSpeed = 0.0f;
        // float verticalSpeed = 2.0f;
        bool movingLeft = false;
        bool movingRight = false;

        float maxMvSpeed = 2.0f;
        bool onGround = false;
        float inertia = 0.05f;
        int playerHurt = 0;
        float jumpHeight = -7.0f;
        float jumpBoostOffEnemy = -10.5f;

        int playerHealth = 2;
        int playerLives = 3;
        int pauseGameTemporarlyAfterDeath = 0;




        float MAX_CAN_MOVE_DOWN = 0;  //need only be int??
        float MAX_CAN_MOVE_UP = 0;
        float MAX_CAN_MOVE_LEFT = 0;
        float MAX_CAN_MOVE_RIGHT = 0;

        int playerWidth = 40;
        int playerHeight = 40;

        Vector2 checkerPntHeadL = new Vector2(100, 100);
        Vector2 checkerPntHeadR = new Vector2(100, 100);
        Vector2 checkerPntFeetL = new Vector2(100, 100);
        Vector2 checkerPntFeetR = new Vector2(100, 100);
        Vector2 checkerPntTorsoL = new Vector2(100, 100);
        Vector2 checkerPntTorsoR = new Vector2(100, 100);

        Vector2 futureCheckerPntHeadL = new Vector2(100, 100);
        Vector2 futureCheckerPntHeadR = new Vector2(100, 100);
        Vector2 futureCheckerPntFeetL = new Vector2(100, 100);
        Vector2 futureCheckerPntFeetR = new Vector2(100, 100);
        Vector2 futureCheckerPntTorsoL = new Vector2(100, 100);
        Vector2 futureCheckerPntTorsoR = new Vector2(100, 100);

        //variables used for loading and managing the levels

        //number of tiles across the level
        const long tilesAcross = 500;    //20,000 pixels across
        //number of tiles down the level
        const long tilesDown = 150;    //6,000 pixels down

        int[,] level = new int[tilesDown, tilesAcross];    //2 dimensional array

        string conversion_string = "";
        string random_string;
        int conversion_int = 0;
        bool debugOn = false;

        const int no_solidTiles = 48; //amount of tiles in "solid" directory
        const int no_passableTiles = 355; //amount of tiles in "passable" directory

        Texture2D[] solidTiles = new Texture2D[no_solidTiles];
        Texture2D[] passableTiles = new Texture2D[no_passableTiles];

        Vector2 playerStartPos = new Vector2(40, 40);
        // Rectangle playerStartingPosRect = new Rectangle(0, 0, 32, 56);

        Texture2D collectableSprite;
        Texture2D levelExitSprite;
        Texture2D oneUpSprite;
        Texture2D healthUpSprite;


        //frog boss level variables
        Texture2D frogBoss;
        Rectangle[] frogBossFrames = new Rectangle[2];
        Vector2 frogBossPos = new Vector2(-500, -50);
        short frogBossFrameNumber = 1;
        Rectangle frogBossRect = new Rectangle();
        Rectangle frogBossHitRect = new Rectangle();
        int frogBossTimer = 0;
        bool frogBossActive = false;
        int frogBossEnergy = 5;
        int frogBossHurt = 0;
        //frogs flame
        Texture2D frogFlameSprite;
        Rectangle[] frogFlameFrames = new Rectangle[2];
        short frogFlameFrameNumber = 0;
        short frogFlameAnimationSpeed = 0;
        Vector2 frogFlamePos = new Vector2(-500, -500);
        bool frogFlameActive = false;
        Rectangle frogFlameRect = new Rectangle();
        Rectangle frogFlameHitRect = new Rectangle();


        //coco nut
        Texture2D cocoNutSprite;
        Rectangle[] cocoNutFrames = new Rectangle[4];

        //flame
        Texture2D flameSprite;
        Rectangle[] flameFrames = new Rectangle[2];
        short flameFrameNumber = 0;
        short flameAnimationSpeed = 0;


        //load textures for backgrounds
        Texture2D[] bg = new Texture2D[13];  //number of backgrounds in directory


        Texture2D titleScreen;
        Texture2D gameOver;

        SpriteFont text1;
        SpriteFont textSmall;
        SpriteFont courier;

        int tempScreenTimer = 0;


        //sizes of sprites
        const int tileWidth = 40;
        const int tileHeight = 40;

        const int collectableWidth = 12;
        const int collectableHeight = 12;

        //scrolling
        public string scrollingType = "none";
        Vector2 scrollerPos = new Vector2(0, 0);
        Vector2 offSet = new Vector2(0, 0);
        int shakeTimer = 0;


        //variable to toggle pause
        bool paused = false;

        //variable to limit the amount of signals sent by keys\
        bool keyPressed = false;
        bool tabKeyPressed = false;
        bool upKeyPressed = false;
        bool downKeyPressed = false;
        bool leftKeyPressed = false;
        bool rightKeyPressed = false;
        bool enterKeyPressed = false;
        bool escKeyPressed = false;


        //used for iterating through the levels
        int levelNumber = 97;   //set to 97 initially as 97 is the title screen level number
        int previousLevelNumber = 0; //used to send the player to title screen, level start screen ect

        //string levelName = ""; //used for displaying the levels name on lev start screen


        //cocoNuts
        struct cocoNut
        {
            public Vector2 pos;
            public bool active;
            public bool falling;
            public Rectangle cocoNutRect;
            public short frameNumber;
        }

        cocoNut[] cocoNuts = new cocoNut[9];

        //flames
        struct flame
        {
            public Vector2 pos;
            public bool active;
            public bool droping;
            public Rectangle flameRect;
            public Rectangle flameHitRect;
            public short frameNumber;
        }

        flame[] flames = new flame[20];

        //collectable

        struct collectable
        {
            public Vector2 pos;
            public bool active;
            public Rectangle collectableRect;
            public float xMovement;
            public float yMovement;
            public int life;
            public float gravity;
            public bool goingRight;
            public float speed;
            public bool isKicked;
        }

        collectable[] collectables = new collectable[200];
        int collectableCounter = 0;

        //background rectangles and variables
        Rectangle bgMoving1 = new Rectangle(0, 0, 800, 600);

        Rectangle bgStatic = new Rectangle(0, 0, 800, 600);

        Rectangle bgMoving2 = new Rectangle(0, 0, 800, 600);


        Vector2 bgMoving1Pos = new Vector2(0, 0);
        Vector2 bgMoving2Pos = new Vector2(0, 0);

        int bgStaticToShow = 1;
        int bgMovingToShow = 6;


        //levelExit
        Vector2 levelExitPos = new Vector2(-100, -100);
        Rectangle levelExitRect = new Rectangle();


        //1up
        struct oneUp
        {
            public Vector2 pos;
            public bool active;
            public Rectangle oneUpRect;
        }

        oneUp[] oneUps = new oneUp[3];

        //health up
        struct healthUp
        {
            public Vector2 pos;
            public bool active;
            public Rectangle healthUpRect;
        }

        healthUp[] healthUps = new healthUp[3];


        #region initialise spikeUps

        Texture2D spikeUpSprite;
        Rectangle spikeUpRect;

        //spikeUps (before load content)
        struct spikeUp
        {
            public Vector2 pos;
            public Rectangle spikeUpRect;
            public Rectangle spikeUpHitRect;
            public bool active;
        }

        spikeUp[] spikeUps = new spikeUp[50];

        #endregion

        #region initialise spikeDowns

        Texture2D spikeDownSprite;
        Rectangle spikeDownRect;

        //spikeDowns (before load content)
        struct spikeDown
        {
            public Vector2 pos;
            public Rectangle spikeDownRect;
            public Rectangle spikeDownHitRect;
            public bool active;
        }

        spikeDown[] spikeDowns = new spikeDown[50];

        #endregion

        #region initialise springMushrooms

        //defing the springMushrooms width and height (before load content)

        const int springMushroomWidth = 80;
        const int springMushroomHeight = 71;

        //springMushroom (before load content)
        Texture2D springMushroomSprite;
        Rectangle[] springMushroomFrames = new Rectangle[3];
        short springMushroomFrameNumber = 0;
        short springMushroomAnimationSpeed = 0;


        //springMushrooms (before load content)
        struct springMushroom
        {
            public Vector2 pos;
            public bool active;
            public Rectangle springMushroomRect;
            public Rectangle springMushroomHitRect;
            public short frameNumber;
            public short animationSpeed;

        }

        springMushroom[] springMushrooms = new springMushroom[10];

        #endregion




        #region initialise bees

        //defing the bees width and height (before load content)

        const int beeWidth = 81;
        const int beeHeight = 70;

        //bee (before load content)
        Texture2D beeSprite;
        Rectangle[] beeFrames = new Rectangle[4];
        short beeFrameNumber = 0;
        short beeAnimationSpeed = 0;


        //bees (before load content)
        struct bee
        {
            public Vector2 pos;
            public bool active;
            public Rectangle beeRect;
            public Rectangle beeHitRect;
            public short frameNumber;
            public bool goingRight;
            public int heightModifier;
        }

        bee[] bees = new bee[10];

        #endregion

        #region initialise birds

        //defing the birds width and height (before load content)

        const int birdWidth = 98;
        const int birdHeight = 58;

        //bird (before load content)
        Texture2D birdSprite;
        Rectangle[] birdFrames = new Rectangle[8];
        short birdFrameNumber = 0;
        short birdAnimationSpeed = 0;


        //birds (before load content)
        struct bird
        {
            public Vector2 pos;
            public bool active;
            public Rectangle birdRect;
            public Rectangle birdHitRect;
            public short frameNumber;
            public bool goingRight;
            public int heightModifier;
        }

        bird[] birds = new bird[10];

        #endregion

        #region initialise ducks

        //defing the ducks width and height (before load content)

        const int duckWidth = 114;
        const int duckHeight = 100;

        //duck (before load content)
        Texture2D duckSprite;
        Rectangle[] duckFrames = new Rectangle[8];
        short duckFrameNumber = 0;
        short duckAnimationSpeed = 0;


        //ducks (before load content)
        struct duck
        {
            public Vector2 pos;
            public bool active;
            public Rectangle duckRect;
            public Rectangle duckHitRect;
            public short frameNumber;
            public bool goingRight;
        }

        duck[] ducks = new duck[10];

        #endregion

        #region initialise fishs

        //defing the fishs width and height (before load content)

        const int fishWidth = 48;
        const int fishHeight = 110;

        //fish (before load content)
        Texture2D fishSprite;
        Rectangle[] fishFrames = new Rectangle[4];
        short fishFrameNumber = 0;
        short fishAnimationSpeed = 0;


        //fishs (before load content)
        struct fish
        {
            public Vector2 pos;
            public bool active;
            public Rectangle fishRect;
            public Rectangle fishHitRect;
            public short frameNumber;
            public bool goingUp;
        }

        fish[] fishs = new fish[10];

        #endregion

        #region initialise hedgehogs

        //defing the hedgehogs width and height (before load content)

        const int hedgehogWidth = 77;
        const int hedgehogHeight = 46;

        //hedgehog (before load content)
        Texture2D hedgehogSprite;
        Rectangle[] hedgehogFrames = new Rectangle[4];
        short hedgehogFrameNumber = 0;
        short hedgehogAnimationSpeed = 0;


        //hedgehogs (before load content)
        struct hedgehog
        {
            public Vector2 pos;
            public bool active;
            public Rectangle hedgehogRect;
            public Rectangle hedgehogHitRect;
            public short frameNumber;
            public bool goingRight;
        }

        hedgehog[] hedgehogs = new hedgehog[10];

        #endregion

        #region initialise ladybirds

        //defing the ladybirds width and height (before load content)

        const int ladybirdWidth = 73;
        const int ladybirdHeight = 25;

        //ladybird (before load content)
        Texture2D ladybirdSprite;
        Rectangle[] ladybirdFrames = new Rectangle[4];
        short ladybirdFrameNumber = 0;
        short ladybirdAnimationSpeed = 0;


        //ladybirds (before load content)
        struct ladybird
        {
            public Vector2 pos;
            public bool active;
            public Rectangle ladybirdRect;
            public Rectangle ladybirdHitRect;
            public short frameNumber;
            public bool goingRight;
            public Rectangle jumpOnLadybirdRect;
        }

        ladybird[] ladybirds = new ladybird[10];

        #endregion

        #region initialise sheeps

        //defing the sheeps width and height (before load content)

        const int sheepWidth = 102;
        const int sheepHeight = 57;

        //sheep (before load content)
        Texture2D sheepSprite;
        Rectangle[] sheepFrames = new Rectangle[4];
        short sheepFrameNumber = 0;
        short sheepAnimationSpeed = 0;


        //sheeps (before load content)
        struct sheep
        {
            public Vector2 pos;
            public bool active;
            public Rectangle sheepRect;
            public Rectangle sheepHitRect;
            public short frameNumber;
            public bool goingRight;
        }

        sheep[] sheeps = new sheep[10];

        #endregion

        #region initialise snails

        //defing the snails width and height (before load content)

        const int snailWidth = 131;
        const int snailHeight = 85;

        //snail (before load content)
        Texture2D snailSprite;
        Rectangle[] snailFrames = new Rectangle[10];
        short snailFrameNumber = 0;
        short snailAnimationSpeed = 0;


        //snails (before load content)
        struct snail
        {
            public Vector2 pos;
            public bool active;
            public Rectangle snailRect;
            public Rectangle snailHitRect;
            public short frameNumber;
            public bool goingRight;
        }

        snail[] snails = new snail[10];

        #endregion

        #region initialise turtles

        //defing the turtles width and height (before load content)

        const int turtleWidth = 51;
        const int turtleHeight = 28;

        //turtle (before load content)
        Texture2D turtleSprite;
        Rectangle[] turtleFrames = new Rectangle[6];
        short turtleFrameNumber = 0;
        short turtleAnimationSpeed = 0;


        //turtles (before load content)
        struct turtle
        {
            public Vector2 pos;
            public bool active;
            public Rectangle turtleRect;
            public Rectangle turtleHitRect;
            public short frameNumber;
            public bool goingRight;
            public Rectangle jumpOnTurtleRect;
        }

        turtle[] turtles = new turtle[10];

        #endregion

        #region initialise sheepBigs

        //defing the sheepBigs width and height (before load content)

        const int sheepBigWidth = 132;
        const int sheepBigHeight = 98;

        //sheepBig (before load content)
        Texture2D sheepBigSprite;
        Rectangle[] sheepBigFrames = new Rectangle[4];
        short sheepBigFrameNumber = 0;
        short sheepBigAnimationSpeed = 0;


        //sheepBigs (before load content)
        struct sheepBig
        {
            public Vector2 pos;
            public bool active;
            public Rectangle sheepBigRect;
            public Rectangle sheepBigHitRect;
            public short frameNumber;
            public bool goingRight;
            public int energy;
            public int hurt;
        }

        sheepBig[] sheepBigs = new sheepBig[1];

        #endregion

        #region initialise duckBigs

        //defing the duckBigs width and height (before load content)

        const int duckBigWidth = 190;
        const int duckBigHeight = 201;

        //duckBig (before load content)
        Texture2D duckBigSprite;
        Rectangle[] duckBigFrames = new Rectangle[8];
        short duckBigFrameNumber = 0;
        short duckBigAnimationSpeed = 0;


        //duckBigs (before load content)
        struct duckBig
        {
            public Vector2 pos;
            public bool active;
            public Rectangle duckBigRect;
            public Rectangle duckBigHitRect;
            public short frameNumber;
            public bool goingRight;
            public int energy;
            public int hurt;
        }

        duckBig[] duckBigs = new duckBig[1];

        #endregion

        //used for level names text
        string[] levelNames = new string[25];


        //music and sfx
        Song musicToPlay;

        SoundEffect collectDotSfx;
        SoundEffect ballKickSfx;
        SoundEffect bossDieSfx;
        SoundEffect collectCoconutSfx;
        SoundEffect flameSfx;
        SoundEffect frogBossJumpSfx;
        SoundEffect frogBossLandSfx;
        SoundEffect healthUpSfx;
        SoundEffect jumpSfx;
        SoundEffect jumpOnBossSfx;
        SoundEffect jumpOnEnemySfx;
        SoundEffect lightningSfx;
        SoundEffect menuSfx;
        SoundEffect menuConfirmSfx;
        SoundEffect oneUpSfx;
        SoundEffect ouchSfx;
        SoundEffect playerDieSfx;
        SoundEffect springSfx;

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load intro assets
            blackBorder = Content.Load<Texture2D>("otherGraph\\blackBorder");

            kickRed = Content.Load<Texture2D>("otherGraph\\kickRed");
            kickBlue = Content.Load<Texture2D>("otherGraph\\kickBlue");
            introBall = Content.Load<Texture2D>("otherGraph\\introBall");
            introLightning = Content.Load<Texture2D>("otherGraph\\introLightning");

            introText0 = Content.Load<Texture2D>("otherGraph\\introText0");
            introText1 = Content.Load<Texture2D>("otherGraph\\introText1");
            introText2 = Content.Load<Texture2D>("otherGraph\\introText2");
            introText3 = Content.Load<Texture2D>("otherGraph\\introText3");
            introText4 = Content.Load<Texture2D>("otherGraph\\introText4");
            introText5 = Content.Load<Texture2D>("otherGraph\\introText5");
            introText6 = Content.Load<Texture2D>("otherGraph\\introText6");
            introText7 = Content.Load<Texture2D>("otherGraph\\introText7");
            introText8 = Content.Load<Texture2D>("otherGraph\\introText8");
            introText9 = Content.Load<Texture2D>("otherGraph\\introText9");
            introText10 = Content.Load<Texture2D>("otherGraph\\introText10");

            //music and sfx
            musicToPlay = Content.Load<Song>("sound\\music\\titleScreen");


            jumpOnEnemySfx = Content.Load<SoundEffect>("sound\\fx\\jumpOnEnemy");
            collectDotSfx = Content.Load<SoundEffect>("sound\\fx\\collectDot");
            ballKickSfx = Content.Load<SoundEffect>("sound\\fx\\ballKick");
            bossDieSfx = Content.Load<SoundEffect>("sound\\fx\\bossDie");
            collectCoconutSfx = Content.Load<SoundEffect>("sound\\fx\\collectCoconut");
            flameSfx = Content.Load<SoundEffect>("sound\\fx\\flame");
            frogBossJumpSfx = Content.Load<SoundEffect>("sound\\fx\\frogBossJump");
            frogBossLandSfx = Content.Load<SoundEffect>("sound\\fx\\frogBossLand");
            healthUpSfx = Content.Load<SoundEffect>("sound\\fx\\healthUp");
            jumpSfx = Content.Load<SoundEffect>("sound\\fx\\jump");
            jumpOnBossSfx = Content.Load<SoundEffect>("sound\\fx\\jumpOnBoss");
            lightningSfx = Content.Load<SoundEffect>("sound\\fx\\lightning");
            menuSfx = Content.Load<SoundEffect>("sound\\fx\\menu");
            menuConfirmSfx = Content.Load<SoundEffect>("sound\\fx\\menuConfirm");
            oneUpSfx = Content.Load<SoundEffect>("sound\\fx\\oneUp");
            ouchSfx = Content.Load<SoundEffect>("sound\\fx\\ouch");
            playerDieSfx = Content.Load<SoundEffect>("sound\\fx\\playerDie");
            springSfx = Content.Load<SoundEffect>("sound\\fx\\spring");

            for (int i = 0; i < 8; i++)
            {
                kickRedRect[i] = new Rectangle(i * 40, 0, 40, 40);
                kickBlueRect[i] = new Rectangle(i * 40, 0, 40, 40);
            }

            for (int i = 0; i < 4; i++)
            {
                introBallRect[i] = new Rectangle(i * 32, 0, 32, 32);

            }

            #region load textures

            text1 = Content.Load<SpriteFont>("text1");
            textSmall = Content.Load<SpriteFont>("textSmall");
            courier = Content.Load<SpriteFont>("courier");

            collectableSprite = Content.Load<Texture2D>("sprites\\collectable");
            levelExitSprite = Content.Load<Texture2D>("sprites\\levelExit");
            oneUpSprite = Content.Load<Texture2D>("sprites\\oneUp");
            healthUpSprite = Content.Load<Texture2D>("sprites\\healthUp");
            frogBoss = Content.Load<Texture2D>("sprites\\frogBoss");
            cocoNutSprite = Content.Load<Texture2D>("sprites\\cocoNut");
            flameSprite = Content.Load<Texture2D>("sprites\\flame");
            frogFlameSprite = Content.Load<Texture2D>("sprites\\flameBig");
            duckSprite = Content.Load<Texture2D>("sprites\\duck");
            beeSprite = Content.Load<Texture2D>("sprites\\bee");
            birdSprite = Content.Load<Texture2D>("sprites\\bird");
            fishSprite = Content.Load<Texture2D>("sprites\\fish");
            hedgehogSprite = Content.Load<Texture2D>("sprites\\hedgehog");
            ladybirdSprite = Content.Load<Texture2D>("sprites\\ladybird");
            sheepSprite = Content.Load<Texture2D>("sprites\\sheep");
            snailSprite = Content.Load<Texture2D>("sprites\\snail");
            turtleSprite = Content.Load<Texture2D>("sprites\\turtle");
            spikeUpSprite = Content.Load<Texture2D>("sprites\\spikeUp");
            spikeDownSprite = Content.Load<Texture2D>("sprites\\spikeDown");
            springMushroomSprite = Content.Load<Texture2D>("sprites\\springMushroom");

            frogBossFrames[0] = new Rectangle(0, 0, 302, 213);
            frogBossFrames[1] = new Rectangle(303, 0, 302, 213);
            sheepBigSprite = Content.Load<Texture2D>("sprites\\sheepBig");
            duckBigSprite = Content.Load<Texture2D>("sprites\\duckBig");

            //load in solid tile graphics
            for (int i = 0; i < no_solidTiles; i++)
            {
                solidTiles[i] = Content.Load<Texture2D>("tiles\\solid\\solidTile" + i);

            }

            //load in passable tile graphics
            for (int i = 0; i < no_passableTiles; i++)
            {

                passableTiles[i] = Content.Load<Texture2D>("tiles\\passable\\passableTile" + i);
            }

            //load sheepBig frames
            for (int i = 0; i < 4; i++)
            {
                sheepBigFrames[i] = new Rectangle(i * sheepBigWidth, 0, sheepBigWidth, sheepBigHeight);
            }

            //load duckBig frames
            for (int i = 0; i < 8; i++)
            {
                duckBigFrames[i] = new Rectangle(i * duckBigWidth, 0, duckBigWidth, duckBigHeight);
            }

            //load titlescreen and menu graphics
            titleScreen = Content.Load<Texture2D>("otherGraph\\titleScreen");
            
            menuMain = Content.Load<Texture2D>("otherGraph\\menuMain");
        menuOptions = Content.Load<Texture2D>("otherGraph\\menuOptions");
        menuPaused = Content.Load<Texture2D>("otherGraph\\menuPaused");
        menuPointer = Content.Load<Texture2D>("otherGraph\\menuPointer");
        menuSlider = Content.Load<Texture2D>("otherGraph\\menuSlider");
        menuSlider2 = Content.Load<Texture2D>("otherGraph\\menuSlider");
        menuExit = Content.Load<Texture2D>("otherGraph\\menuExit");

            //  titleScreen.Dispose();



            #region prepare level names text

            levelNames[0] = "Introduction... press Enter key or X button to skip";
            levelNames[1] = "Level B-1...";
            levelNames[2] = "Level B-2...";
            levelNames[3] = "Level B-3...";
            levelNames[4] = "Level B-4...";
            levelNames[5] = "Level C-1...";
            levelNames[6] = "Level T-1...";
            levelNames[7] = "Level F-1...";
            levelNames[8] = "Level F-2...";
            levelNames[9] = "Level C-2...";
            levelNames[10] = "Level T-2...";
            levelNames[11] = "Level M-1...";
            levelNames[12] = "Level M-2...";
            levelNames[13] = "Level C-3...";
            levelNames[14] = "Level T-3...";
            levelNames[15] = "Level M-3...";
            levelNames[16] = "Level S-1...";
            levelNames[17] = "Level S-2...";
            levelNames[18] = "Level F-3...";
            levelNames[19] = "Level FB...";
            levelNames[20] = "Congratulations!";
            levelNames[21] = "Level I-1...";
            levelNames[22] = "Level I-2...";
            levelNames[23] = "Level C-4...";
            levelNames[24] = "Level UM...";

            #endregion

            //load game over screen graphics
            gameOver = Content.Load<Texture2D>("otherGraph\\gameOver");

            //hud graphics
            healthMeter = Content.Load<Texture2D>("otherGraph\\healthMeter");
            healthMeter = Content.Load<Texture2D>("otherGraph\\healthMeter");


            //backgrounds
            bg[0] = Content.Load<Texture2D>("backgrounds\\bg0");
            bg[1] = Content.Load<Texture2D>("backgrounds\\bg1");
            bg[2] = Content.Load<Texture2D>("backgrounds\\bg2");
            bg[3] = Content.Load<Texture2D>("backgrounds\\bg3");
            bg[4] = Content.Load<Texture2D>("backgrounds\\bg4");
            bg[5] = Content.Load<Texture2D>("backgrounds\\bg5");
            bg[6] = Content.Load<Texture2D>("backgrounds\\bg6");
            bg[7] = Content.Load<Texture2D>("backgrounds\\bg7");
            bg[8] = Content.Load<Texture2D>("backgrounds\\bg8");
            bg[9] = Content.Load<Texture2D>("backgrounds\\bg9");
            bg[10] = Content.Load<Texture2D>("backgrounds\\bg10");
            bg[11] = Content.Load<Texture2D>("backgrounds\\bg11");
            bg[12] = Content.Load<Texture2D>("backgrounds\\bg12");

            //load coconut frames
            for (int i = 0; i < 4; i++)
            {
                cocoNutFrames[i] = new Rectangle(i * 32, 0, 32, 32);
            }

            //load flame frames
            for (int i = 0; i < 2; i++)
            {
                flameFrames[i] = new Rectangle(i * 40, 0, 40, 40);
            }

            //load frogFlame frames
            for (int i = 0; i < 2; i++)
            {
                frogFlameFrames[i] = new Rectangle(i * 80, 0, 80, 80);
            }

            //load mrstick sprite frames
            mrstickSprite = Content.Load<Texture2D>("sprites\\mrstick");

            for (int i = 0; i < 48; i++)
            {
                mrstickFrames[i] = new Rectangle(i * 40, 0, 40, 40);
            }

            //load health meter frames
            for (int i = 0; i < 3; i++)
            {
                healthMeterRect[i] = new Rectangle(i * 40, 0, 40, 40);
            }

            //load duck frames
            for (int i = 0; i < 8; i++)
            {
                duckFrames[i] = new Rectangle(i * duckWidth, 0, duckWidth, duckHeight);
            }

            //load hedgehog frames
            for (int i = 0; i < 4; i++)
            {
                hedgehogFrames[i] = new Rectangle(i * hedgehogWidth, 0, hedgehogWidth, hedgehogHeight);
            }

            //load ladybird frames
            for (int i = 0; i < 4; i++)
            {
                ladybirdFrames[i] = new Rectangle(i * ladybirdWidth, 0, ladybirdWidth, ladybirdHeight);
            }

            //load sheep frames
            for (int i = 0; i < 4; i++)
            {
                sheepFrames[i] = new Rectangle(i * sheepWidth, 0, sheepWidth, sheepHeight);
            }

            //load snail frames
            for (int i = 0; i < 10; i++)
            {
                snailFrames[i] = new Rectangle(i * snailWidth, 0, snailWidth, snailHeight);
            }

            for (int i = 0; i < 6; i++)
            {
                turtleFrames[i] = new Rectangle(i * turtleWidth, 0, turtleWidth, turtleHeight);
            }

            //load bee frames
            for (int i = 0; i < 4; i++)
            {
                beeFrames[i] = new Rectangle(i * beeWidth, 0, beeWidth, beeHeight);
            }

            //load bird frames
            for (int i = 0; i < 8; i++)
            {
                birdFrames[i] = new Rectangle(i * birdWidth, 0, birdWidth, birdHeight);
            }

            //load fish frames
            for (int i = 0; i < 4; i++)
            {
                fishFrames[i] = new Rectangle(i * fishWidth, 0, fishWidth, fishHeight);
            }

            //load springMushroom frames
            for (int i = 0; i < 3; i++)
            {
                springMushroomFrames[i] = new Rectangle(i * springMushroomWidth, 0, springMushroomWidth, springMushroomHeight);
            }

            spikeDownRect = new Rectangle(0, 0, 12, 12);
            spikeUpRect = new Rectangle(0, 0, 12, 12);

            #endregion

            //startOfLevel();


            loadLevel("level" + levelNumber);


        }

        private void startOfLevel()
        {

            MediaPlayer.Stop();
            MediaPlayer.Volume = musicVol;
            

            //initialise the player movent variables
            moveSpeed = 0.0f;
            gravity = 0.0f;
            playerHurt = 0;

            if (levelNumber != 3)
            {
                playerFacingRight = true;
            }

            if (levelNumber == 3)
            {
                playerFacingRight = false;
            }

            if (levelNumber == 95)
            {
                introTimer = 0;

                kickRedPos.X = 6480;
                kickRedPos.Y = 1640;
                kickBluePos.X = 7080;
                kickBluePos.Y = 1640;
                introBallPos.X = 6580;
                introBallPos.Y = 1650;
            }

            if (levelNumber == 96)
            {
                endingTimer = 0;

                kickRedPos.X = 6560;
                kickRedPos.Y = 1640;
                kickBluePos.X = 7000;
                kickBluePos.Y = 1640;

                introBallPos.X = 6860;
                introBallPos.Y = 1648;
            }


            //prepare sheepBigs for use in level
            if (levelNumber == 6)
            {
                sheepBigs[0].pos.X = 2460;
                sheepBigs[0].pos.Y = 4942;
                sheepBigs[0].frameNumber = 0;
                sheepBigs[0].hurt = 0;
                sheepBigs[0].energy = 5;

                if (sheepBigs[0].pos.X > 0)
                {
                    sheepBigs[0].active = true;
                    sheepBigs[0].goingRight = false;

                }
                else
                {
                    sheepBigs[0].active = false;
                }


            }

            //prepare duckBigs for use in level
            if (levelNumber == 10)
            {
                duckBigs[0].pos.X = 2260;
                duckBigs[0].pos.Y = 4839;
                duckBigs[0].frameNumber = 0;
                duckBigs[0].hurt = 0;
                duckBigs[0].energy = 5;

                if (duckBigs[0].pos.X > 0)
                {
                    duckBigs[0].active = true;
                    duckBigs[0].goingRight = false;

                }
                else
                {
                    duckBigs[0].active = false;
                }


            }

            //prepare level 14 ( 2 bosses)
            if (levelNumber == 14)
            {
                sheepBigs[0].pos.X = 2720;
                sheepBigs[0].pos.Y = 4942;
                sheepBigs[0].frameNumber = 0;
                sheepBigs[0].hurt = 0;
                sheepBigs[0].energy = 5;

                if (sheepBigs[0].pos.X > 0)
                {
                    sheepBigs[0].active = true;
                    sheepBigs[0].goingRight = false;

                }
                else
                {
                    sheepBigs[0].active = false;
                }



                duckBigs[0].pos.X = 4240;
                duckBigs[0].pos.Y = 4839;
                duckBigs[0].frameNumber = 0;
                duckBigs[0].hurt = 0;
                duckBigs[0].energy = 5;

                if (duckBigs[0].pos.X > 0)
                {
                    duckBigs[0].active = true;
                    duckBigs[0].goingRight = true;

                }
                else
                {
                    duckBigs[0].active = false;
                }

            }

            //initialise coconuts

            // if(level==sandlevel)
            for (int i = 0; i < 9; i++)
            {
                if (cocoNuts[i].pos.X > -500)
                {
                    cocoNuts[i].active = true;
                }


                cocoNuts[i].falling = false;
                cocoNuts[i].frameNumber = 0;

            }

            //initialise flames

            for (int i = 0; i < 20; i++)
            {
                if (flames[i].pos.X > 0)
                {
                    flames[i].active = true;
                }
                else
                {
                    flames[i].active = false;
                }

                flames[i].droping = false;
                flames[i].frameNumber = 0;

            }



            //initialise spikeUps

            for (int i = 0; i < 50; i++)
            {
                spikeUps[i].active = true;

            }

            //prepare springMushrooms for use in level

            for (int i = 0; i < 10; i++)
            {
                if (springMushrooms[i].pos.X > 0)
                {
                    springMushrooms[i].active = true;
                    springMushrooms[i].frameNumber = 0;
                    springMushrooms[i].animationSpeed = 0;
                }

            }

            //initialise spikeDowns

            for (int i = 0; i < 50; i++)
            {
                spikeDowns[i].active = true;

            }


            //initialise dots (collectables)
            for (int i = 0; i < 200; i++)
            {
                if (collectables[i].pos.X >= 0)
                {
                    collectables[i].active = true;

                }
            }


            //initialise 1ups and health ups
            for (int i = 0; i < 3; i++)
            {
                oneUps[i].active = true;
                healthUps[i].active = true;
            }


            //prepare ducks for use in level

            for (int i = 0; i < 10; i++)
            {
                if (ducks[i].pos.X > 0)
                {
                    ducks[i].active = true;
                    ducks[i].goingRight = false;
                }
                else
                {
                    ducks[i].active = false;
                }

                ducks[i].frameNumber = 0;

            }

            //prepare hedgehogs for use in level

            for (int i = 0; i < 10; i++)
            {
                if (hedgehogs[i].pos.X > 0)
                {
                    hedgehogs[i].active = true;
                    hedgehogs[i].goingRight = false;
                }
                else
                {
                    hedgehogs[i].active = false;
                }

                hedgehogs[i].frameNumber = 0;

            }

            //prepare ladybirds for use in level

            for (int i = 0; i < 10; i++)
            {
                if (ladybirds[i].pos.X > 0)
                {
                    ladybirds[i].active = true;
                    ladybirds[i].goingRight = false;
                }
                else
                {
                    ladybirds[i].active = false;
                }

                ladybirds[i].frameNumber = 0;

            }

            //prepare sheeps for use in level

            for (int i = 0; i < 10; i++)
            {
                if (sheeps[i].pos.X > 0)
                {
                    sheeps[i].active = true;
                    sheeps[i].goingRight = false;
                }
                else
                {
                    sheeps[i].active = false;
                }

                sheeps[i].frameNumber = 0;

            }


            //prepare snails for use in level

            for (int i = 0; i < 10; i++)
            {
                if (snails[i].pos.X > 0)
                {
                    snails[i].active = true;
                    snails[i].goingRight = false;
                }
                else
                {
                    snails[i].active = false;
                }

                snails[i].frameNumber = 0;

            }

            //prepare turtles for use in level

            for (int i = 0; i < 10; i++)
            {
                if (turtles[i].pos.X > 0)
                {
                    turtles[i].active = true;
                    turtles[i].goingRight = false;
                }
                else
                {
                    turtles[i].active = false;
                }

                turtles[i].frameNumber = 0;

            }

            //prepare bees for use in level

            for (int i = 0; i < 10; i++)
            {
                if (bees[i].pos.X > 0)
                {
                    bees[i].active = true;
                    bees[i].goingRight = false;
                    bees[i].heightModifier = 120;
                }
                else
                {
                    bees[i].active = false;
                }

                bees[i].frameNumber = 0;

            }

            //prepare birds for use in level

            for (int i = 0; i < 10; i++)
            {
                if (birds[i].pos.X > 0)
                {
                    birds[i].active = true;
                    birds[i].goingRight = false;
                    birds[i].heightModifier = 120;
                }
                else
                {
                    birds[i].active = false;
                }

                birds[i].frameNumber = 0;

            }

            //prepare fishs for use in level

            for (int i = 0; i < 10; i++)
            {
                if (fishs[i].pos.X > 0)
                {
                    fishs[i].active = true;
                    fishs[i].goingUp = true;
                }
                else
                {
                    fishs[i].active = false;
                }

                fishs[i].frameNumber = 0;

            }

            //prepare frogboss level
            if (levelNumber == 19)
            {
                frogBossPos.X = 3760;
                frogBossPos.Y = 2587;
                frogBossHurt = 0;
                frogBossEnergy = 5;
                frogBossActive = true;
                frogBossTimer = 0;
                frogFlameActive = false;
                frogFlameAnimationSpeed = 0;
                frogFlameFrameNumber = 0;
                frogBossFrameNumber = 1;
            }


            //start music for levels
            if (levelNumber == 97)
            {

                musicToPlay = Content.Load<Song>("sound\\music\\titleScreen");
                optionSelected = "startGame";

                menuShowing = "main";

                
            }

            if (levelNumber == 95)
            {

                musicToPlay = Content.Load<Song>("sound\\music\\intro");
                

            }

            if (levelNumber == 99)
            {

                musicToPlay = Content.Load<Song>("sound\\music\\levelStart");
                

            }

            if (levelNumber == 96)
            {

                musicToPlay = Content.Load<Song>("sound\\music\\ending");

            }

            if (levelNumber == 98)
            {

                musicToPlay = Content.Load<Song>("sound\\music\\gameOver");

            }

            if (levelNumber == 1 || levelNumber == 2 || levelNumber == 3 || levelNumber == 4)
            {
                musicToPlay = Content.Load<Song>("sound\\music\\beach");

            }

            if (levelNumber == 5 || levelNumber == 9 || levelNumber == 13)
            {

                musicToPlay = Content.Load<Song>("sound\\music\\cave");

            }

            if (levelNumber == 6 || levelNumber == 10 || levelNumber == 14)
            {

                musicToPlay = Content.Load<Song>("sound\\music\\boss");

            }

            if (levelNumber == 7 || levelNumber == 8 || levelNumber == 18)
            {

                musicToPlay = Content.Load<Song>("sound\\music\\forest");
                
            }

            if (levelNumber == 11 || levelNumber == 12 || levelNumber == 15)
            {

                musicToPlay = Content.Load<Song>("sound\\music\\mushroom");

            }

            if (levelNumber == 16 || levelNumber == 17)
            {

                musicToPlay = Content.Load<Song>("sound\\music\\cloud");

            }

            if (levelNumber == 19)
            {

                musicToPlay = Content.Load<Song>("sound\\music\\frogBoss");

            }

            MediaPlayer.Play(musicToPlay);
            MediaPlayer.IsRepeating = true;
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            #region Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();

            //if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            //{
            //    this.Exit();
            //}

            controlMainMenuExitGame();
            #endregion


            //game will run if the player isnt hurt or if it isnt paused
            if (paused == false && playerHurt < 180 && pauseGameTemporarlyAfterDeath == 0)
            {

                updateSprites();

                if (levelNumber < 90)
                {
                    doPlayer(); //run method to handel main sprite controls

                    scrolling(); //run method that handels scrolling


                    doDots();


                    doEnemys();
                }

                offSet.X = (int)scrollerPos.X - 400;
                offSet.Y = (int)scrollerPos.Y - 300;


            }

            controlMainGameLoop();


            //press esc key to toggle pause
            if (levelNumber < 90)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && escKeyPressed == false && pauseGameTemporarlyAfterDeath==0 /*&& paused==false*/)
                {
                    
                    escKeyPressed = true;
                    paused = !paused;

                    menuShowing = "paused";
                    optionSelected = "resume";
                    menuPointerPos.X = 308; menuPointerPos.Y = 305;

                    if (paused == false)
                    {
                        menuShowing = "";
                    }
                    
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Escape))
                {
                    escKeyPressed = false;
                }


            }

            


            //if the player is hurt the game will pause momentarally
            if (playerHurt > 0)
            {
                playerHurt--;
            }

            if (playerHurt < 0)
            {
                playerHurt++;
            }

            //if the player dies, pause the game momentarlly
            if (pauseGameTemporarlyAfterDeath > 0)
            {
                pauseGameTemporarlyAfterDeath--;
            }

            if (debugOn == true)
            {
                debug();
            }

            base.Update(gameTime);
        }

        private void controlMainMenuExitGame()
        {
            #region main menu & options


            //main menu
            if (menuShowing == "main" && levelNumber ==97)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && downKeyPressed == false)
                {


                    if (optionSelected == "startGame" && downKeyPressed == false)
                    {
                        optionSelected = "options";
                        downKeyPressed = true;

                        menuPointerPos.X = 284; menuPointerPos.Y = 516;

                        menuSfx.Play(sfxVol);
                    }

                    if (optionSelected == "options" && downKeyPressed == false)
                    {
                        optionSelected = "exit";
                        downKeyPressed = true;

                        menuPointerPos.X = 284; menuPointerPos.Y = 557;

                        menuSfx.Play(sfxVol);
                    }

                    if (optionSelected == "exit" && downKeyPressed == false)
                    {
                        optionSelected = "startGame";
                        downKeyPressed = true;

                        menuPointerPos.X = 284; menuPointerPos.Y = 474;

                        menuSfx.Play(sfxVol);
                    }

                }

                if (Keyboard.GetState().IsKeyUp(Keys.Down))
                {
                    downKeyPressed = false;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                {
                    enterKeyPressed = false;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up) && upKeyPressed == false)
                {


                    if (optionSelected == "startGame" && upKeyPressed == false)
                    {
                        optionSelected = "exit";
                        upKeyPressed = true;

                        menuPointerPos.X = 284; menuPointerPos.Y = 557;

                        menuSfx.Play(sfxVol);
                    }

                    if (optionSelected == "options" && upKeyPressed == false)
                    {
                        optionSelected = "startGame";
                        upKeyPressed = true;

                        menuPointerPos.X = 284; menuPointerPos.Y = 474;

                        menuSfx.Play(sfxVol);
                    }

                    if (optionSelected == "exit" && upKeyPressed == false)
                    {
                        optionSelected = "options";
                        upKeyPressed = true;

                        menuPointerPos.X = 284; menuPointerPos.Y = 516;

                        menuSfx.Play(sfxVol);
                    }

                }

                if (Keyboard.GetState().IsKeyUp(Keys.Up))
                {
                    upKeyPressed = false;
                }

                // go to options
                if (optionSelected=="options" &&
                    Keyboard.GetState().IsKeyDown(Keys.Enter) && enterKeyPressed == false)
                {
                    enterKeyPressed = true;
                    menuShowing = "options";
                    optionSelected = "musicVolume";

                    menuPointerPos.X = 284; menuPointerPos.Y = 474;
                    menuConfirmSfx.Play(sfxVol);

                    
                }


                //go to exit game menu
                if (optionSelected=="exit" && Keyboard.GetState().IsKeyDown(Keys.Enter) && enterKeyPressed == false)
                {
                    enterKeyPressed = true;
                    menuShowing = "exitGame";
                    optionSelected = "yes";
                    menuPointerPos.X = 280; menuPointerPos.Y = 305;
                    menuConfirmSfx.Play(sfxVol);
                }

                //press enter on "startGame" to begin playing

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyPressed == false && optionSelected=="startGame" && enterKeyPressed == false)
                {
                    menuConfirmSfx.Play(sfxVol);
                    enterKeyPressed = true;
                    keyPressed = true;
                    levelNumber = 95; //go to intro

                    previousLevelNumber = 1;

                    loadLevel("level" + levelNumber);
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                {
                    keyPressed = false;
                }


            }


            //options menu
            if (menuShowing == "options" && levelNumber ==97)
            {


                if (Keyboard.GetState().IsKeyDown(Keys.Down) && downKeyPressed == false)
                {


                    if (optionSelected == "musicVolume" && downKeyPressed == false)
                    {
                        optionSelected = "sfxVolume";
                        downKeyPressed = true;

                        menuPointerPos.X = 284; menuPointerPos.Y = 516;

                        menuSfx.Play(sfxVol);
                    }

                    if (optionSelected == "sfxVolume" && downKeyPressed == false)
                    {
                        optionSelected = "return";
                        downKeyPressed = true;

                        menuPointerPos.X = 284; menuPointerPos.Y = 557;

                        menuSfx.Play(sfxVol);
                    }

                    if (optionSelected == "return" && downKeyPressed == false)
                    {
                        optionSelected = "musicVolume";
                        downKeyPressed = true;

                        menuPointerPos.X = 284; menuPointerPos.Y = 474;

                        menuSfx.Play(sfxVol);
                    }

                    
                    

                }

                

                if (Keyboard.GetState().IsKeyUp(Keys.Down))
                {
                    downKeyPressed = false;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                {
                    enterKeyPressed = false;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up) && upKeyPressed == false)
                {



                    if (optionSelected == "musicVolume" && upKeyPressed == false)
                    {
                        optionSelected = "return";
                        upKeyPressed = true;
                        
                        menuPointerPos.X = 284; menuPointerPos.Y = 557;

                        menuSfx.Play(sfxVol);

                    }

                    if (optionSelected == "sfxVolume" && upKeyPressed == false)
                    {
                        optionSelected = "musicVolume";
                        upKeyPressed = true;

                        menuPointerPos.X = 284; menuPointerPos.Y = 474;

                        menuSfx.Play(sfxVol);
                    }

                    if (optionSelected == "return" && upKeyPressed == false)
                    {
                        optionSelected = "sfxVolume";
                        upKeyPressed = true;

                        menuPointerPos.X = 284; menuPointerPos.Y = 516;

                        menuSfx.Play(sfxVol);
                    }

                }

                if (Keyboard.GetState().IsKeyUp(Keys.Up))
                {
                    upKeyPressed = false;
                }

                
                if (optionSelected=="return"
                    && Keyboard.GetState().IsKeyDown(Keys.Enter) && enterKeyPressed == false)
                {
                    enterKeyPressed = true;
                    menuShowing = "main";
                    optionSelected = "startGame";
                    menuPointerPos.X = 284; menuPointerPos.Y = 474;
                    menuConfirmSfx.Play(sfxVol);
                }

                //control sliders

                //decrease sfx volume
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && leftKeyPressed == false)
                {
                    if (optionSelected == "sfxVolume" && sfxVol > 0.0f)
                    {
                        leftKeyPressed = true;

                        sfxVol -= 0.1f;
                        
                        menuSlider2Pos.X -= 10;

                        menuConfirmSfx.Play(sfxVol);
                    }

                    
                }

                //increase sfx volume
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && rightKeyPressed == false)
                {
                    if (optionSelected == "sfxVolume" && sfxVol < 1.0f)
                    {
                        rightKeyPressed = true;

                        sfxVol += 0.1f;
                        
                        menuSlider2Pos.X += 10;

                        menuConfirmSfx.Play(sfxVol);
                    }


                }

                //decrease music volume
                if (Keyboard.GetState().IsKeyDown(Keys.Left) && leftKeyPressed == false)
                {
                    if (optionSelected == "musicVolume" && musicVol > 0.1f)
                    {
                        leftKeyPressed = true;

                        musicVol -= 0.1f;
                        MediaPlayer.Volume = musicVol;
                        menuSlider1Pos.X -= 10;

                        menuConfirmSfx.Play(sfxVol);
                    }


                }

                //increase music volume
                if (Keyboard.GetState().IsKeyDown(Keys.Right) && rightKeyPressed == false)
                {
                    if (optionSelected == "musicVolume" && musicVol < 1.0f)
                    {
                        rightKeyPressed = true;

                        musicVol += 0.1f;
                        MediaPlayer.Volume = musicVol;
                        menuSlider1Pos.X += 10;

                        menuConfirmSfx.Play(sfxVol);
                    }


                }

                

                if (Keyboard.GetState().IsKeyUp(Keys.Left))
                {
                    leftKeyPressed = false;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Right))
                {
                    rightKeyPressed = false;
                }

                

            }

            //exit game menu
            if (menuShowing == "exitGame" && levelNumber ==97)
            {


                if (Keyboard.GetState().IsKeyDown(Keys.Down) && downKeyPressed == false)
                {


                    if (optionSelected == "yes" && downKeyPressed == false)
                    {
                        optionSelected = "no";
                        downKeyPressed = true;

                        menuPointerPos.X = 280; menuPointerPos.Y = 345;

                        menuSfx.Play(sfxVol);
                    }

                    if (optionSelected == "no" && downKeyPressed == false)
                    {
                        optionSelected = "yes";
                        downKeyPressed = true;

                        menuPointerPos.X = 280; menuPointerPos.Y = 305;

                        menuSfx.Play(sfxVol);
                    }

                }

                if (Keyboard.GetState().IsKeyUp(Keys.Down))
                {
                    downKeyPressed = false;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                {
                    enterKeyPressed = false;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up) && upKeyPressed == false)
                {


                    if (optionSelected == "yes" && upKeyPressed == false)
                    {
                        optionSelected = "no";
                        upKeyPressed = true;

                        menuPointerPos.X = 280; menuPointerPos.Y = 345;

                        menuSfx.Play(sfxVol);
                    }

                    if (optionSelected == "no" && upKeyPressed == false)
                    {
                        optionSelected = "yes";
                        upKeyPressed = true;

                        menuPointerPos.X = 280; menuPointerPos.Y = 305;

                        menuSfx.Play(sfxVol);
                    }

                }

                if (Keyboard.GetState().IsKeyUp(Keys.Up))
                {
                    upKeyPressed = false;
                }

                //go back to main menu
                if (optionSelected=="no"
                    && Keyboard.GetState().IsKeyDown(Keys.Enter) && enterKeyPressed == false)
                {
                    enterKeyPressed = true;
                    menuShowing = "main";
                    optionSelected = "startGame";

                    menuPointerPos.X = 284; menuPointerPos.Y = 474;
                    menuConfirmSfx.Play(sfxVol);

                }


                //leave game
                if (optionSelected=="yes" && Keyboard.GetState().IsKeyDown(Keys.Enter) && enterKeyPressed == false)
                {
                    menuConfirmSfx.Play(sfxVol);
                    enterKeyPressed = true;
                    this.Exit();
                }


            }

            //paused menu
            if (menuShowing == "paused" && levelNumber < 90)
            {


                if (Keyboard.GetState().IsKeyDown(Keys.Down) && downKeyPressed == false)
                {


                    if (optionSelected == "resume" && downKeyPressed == false)
                    {
                        optionSelected = "exitToTitle";
                        downKeyPressed = true;

                        menuPointerPos.X = 308; menuPointerPos.Y = 345;

                        menuSfx.Play(sfxVol);
                    }

                    if (optionSelected == "exitToTitle" && downKeyPressed == false)
                    {
                        optionSelected = "resume";
                        downKeyPressed = true;

                        menuPointerPos.X = 308; menuPointerPos.Y = 305;

                        menuSfx.Play(sfxVol);
                    }


                }

                if (Keyboard.GetState().IsKeyUp(Keys.Down))
                {
                    downKeyPressed = false;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                {
                    enterKeyPressed = false;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up) && upKeyPressed == false)
                {


                    if (optionSelected == "resume" && upKeyPressed == false)
                    {
                        optionSelected = "exitToTitle";
                        upKeyPressed = true;

                        menuPointerPos.X = 308; menuPointerPos.Y = 345;

                        menuSfx.Play(sfxVol);
                    }

                    if (optionSelected == "exitToTitle" && upKeyPressed == false)
                    {
                        optionSelected = "resume";
                        upKeyPressed = true;

                        menuPointerPos.X = 308; menuPointerPos.Y = 305;

                        menuSfx.Play(sfxVol);
                    }

                }

                if (Keyboard.GetState().IsKeyUp(Keys.Up))
                {
                    upKeyPressed = false;
                }

                
                //select resume to go back to game
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && enterKeyPressed == false && optionSelected=="resume")
                {
                    enterKeyPressed = true;
                    paused = !paused;
                    menuShowing = "";
                    menuConfirmSfx.Play(sfxVol);
                }

                //select exit to go to title screen
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && enterKeyPressed == false && optionSelected=="exitToTitle")
                {
                    menuConfirmSfx.Play(sfxVol);
                    enterKeyPressed = true;
                    paused = !paused;

                    menuPointerPos.X = 284; menuPointerPos.Y = 474;
                    menuShowing="main";

                    

                    levelExitPos.X = -500;
                    levelExitPos.Y = -500;

                    //reset the playerSprite movement variables
                    gravity = 0.0f;
                    moveSpeed = 0.0f;

                    levelNumber = 97;

                    loadLevel("level" + levelNumber);
                }
            }
            #endregion

            //paused menu
            if (paused == true)
            {
                menuShowing = "paused";
            }


        }

        private void debug()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Tab) && tabKeyPressed == false
                && levelNumber < 90)
            {
                tabKeyPressed = true;

                levelExitPos.X = -500;
                levelExitPos.Y = -500;

                //reset the playerSprite movement variables
                gravity = 0.0f;
                moveSpeed = 0.0f;

                if (levelNumber < 19)
                {
                    levelNumber += 1;
                }

                loadLevel("level" + levelNumber);
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Tab))
            {
                tabKeyPressed = false;
            }


        }

        private void drawDebugInformation()
        {
            //draw player co-ords if debug is on
            if (debugOn == true)
            {
                //if (gravity < 0.0f)
                //{

                //    spriteBatch.DrawString(textSmall, "gravity " + gravity.ToString(), new Vector2(20, 20), Color.Green);
                //    spriteBatch.DrawString(textSmall, "Going UP", new Vector2(20, 50), Color.Green);
                //}

                //if (gravity > 0.0f)
                //{
                //    spriteBatch.DrawString(textSmall, "gravity " + gravity.ToString(), new Vector2(20, 80), Color.Green);
                //    spriteBatch.DrawString(textSmall, "FALLING", new Vector2(20, 110), Color.Green);
                //}

                //if (onGround == true)
                //{
                //    spriteBatch.DrawString(textSmall, "ON ground", new Vector2(20, 140), Color.Green);
                //}

                //if (onGround == false)
                //{
                //    spriteBatch.DrawString(textSmall, "OFF ground", new Vector2(20, 140), Color.Green);
                //}

                //if (movingRight == false)
                //{
                //    spriteBatch.DrawString(textSmall, "moving LEFT", new Vector2(20, 170), Color.Green);
                //}

                //if (movingRight == true)
                //{
                //    spriteBatch.DrawString(textSmall, "moving RIGHT", new Vector2(20, 170), Color.Green);
                //}

                //if (playerFacingRight == false)
                //{
                //    spriteBatch.DrawString(textSmall, "FACING :) LEFT", new Vector2(20, 220), Color.Green);
                //}

                //if (playerFacingRight == true)
                //{
                //    spriteBatch.DrawString(textSmall, "FACING :) RIGHT", new Vector2(20, 220), Color.Green);
                //}

                //spriteBatch.DrawString(textSmall, mrstickFrameNumber.ToString(), new Vector2(20, 250), Color.Green);

                //if (levelNumber == 95)
                //{
                //    spriteBatch.DrawString(text1, introTimer.ToString(), new Vector2(20, 250), Color.Blue);
                //}

                //if (levelNumber == 96)
                //{
                //    spriteBatch.DrawString(text1, endingTimer.ToString(), new Vector2(20, 250), Color.Blue);
                //}

                //spriteBatch.DrawString(text1, menuShowing, new Vector2(10, 10), Color.Green);
                //spriteBatch.DrawString(text1, optionSelected, new Vector2(10, 50), Color.Green);

                //spriteBatch.DrawString(text1, musicVol.ToString(), new Vector2(10, 50), Color.Green);
                //spriteBatch.DrawString(text1, sfxVol.ToString(), new Vector2(10, 90), Color.Pink);

                if (levelNumber == 19)
                {
                    spriteBatch.DrawString(text1, frogBossTimer.ToString(), new Vector2(20, 250), Color.Blue);
                }

               
            }
        }

        private void controlMainGameLoop()
        {

            //title screen code
            if (levelNumber == 97)
            {
                

                score = 0;
                playerLives = 3;
                playerHealth = 2;
                playerHurt = 0;
                introTimer = 0;
                endingTimer = 0;
                collectableCounter = 0;

            }

            //go to level start screen if player is killed

            if (playerHealth == 0 && pauseGameTemporarlyAfterDeath == 0 && playerLives > 1)
            {



                previousLevelNumber = levelNumber;

                playerLives--;
                collectableCounter = 0;
                levelNumber = 99;
                playerHealth = 2;
                playerHurt = 0;

                loadLevel("level" + levelNumber);

            }

            //go to Game Over screen if player is killed with no lives remaining

            if (playerHealth == 0 && pauseGameTemporarlyAfterDeath == 0 && playerLives == 1)
            {



                //previousLevelNumber = levelNumber;

                //playerLives--;
                //levelNumber = 99;
                //playerHealth = 2;
                //playerHurt = 0;



                playerHealth = 2;
                playerHurt = 0;



                levelNumber = 98;   // 98 is the number of the game over screen level

                loadLevel("level" + levelNumber);
            }

            //control level start screen
            if (levelNumber == 99)
            {
                tempScreenTimer++;

                if (tempScreenTimer == 180)
                {
                    levelNumber = previousLevelNumber;

                    tempScreenTimer = 0;

                    loadLevel("level" + levelNumber);
                }
            }

            //control game over screen
            if (levelNumber == 98)
            {


                tempScreenTimer++;

                if (tempScreenTimer == 360)
                {
                    levelNumber = 97;

                    tempScreenTimer = 0;

                    loadLevel("level" + levelNumber);
                }
            }

            //control intro
            if (levelNumber == 95)
            {
                scrolling();
                introTimer++;

                if (introBallGoingRight == true && introTimer < 2300)
                {
                    introBallPos.X += 3;

                    introBallAnimationSpeed++;
                    if (introBallAnimationSpeed >= 8)
                    {
                        introBallAnimationSpeed = 0;
                        introBallFrameNumber++;

                        if (introBallFrameNumber > 3)
                        {
                            introBallFrameNumber = 0;
                        }
                    }
                }

                if (introBallGoingRight == false && introTimer < 2300)
                {
                    introBallPos.X -= 3;

                    introBallAnimationSpeed++;
                    if (introBallAnimationSpeed >= 8)
                    {
                        introBallAnimationSpeed = 0;
                        introBallFrameNumber--;

                        if (introBallFrameNumber < 0)
                        {
                            introBallFrameNumber = 3;
                        }
                    }
                }

                if (introBallPos.X <= 6520)
                {
                    introBallGoingRight = true;
                    kickRedFrameNumber = 1;

                    if (introTimer > 1450)
                    {
                        ballKickSfx.Play(sfxVol);
                    }
                }

                if (introBallPos.X >= 7040)
                {
                    introBallGoingRight = false;
                    kickBlueFrameNumber = 1;

                    if (introTimer > 1450)
                    {
                        ballKickSfx.Play(sfxVol);
                    }
                }



                if (kickRedFrameNumber > 0)
                {
                    kickAnimationSpeed++;

                    if (kickAnimationSpeed == 3)
                    {
                        kickAnimationSpeed = 0;
                        kickRedFrameNumber++;

                        if (kickRedFrameNumber == 8)
                        {
                            kickRedFrameNumber = 0;
                            kickAnimationSpeed = 0;
                        }
                    }
                }

                if (kickBlueFrameNumber > 0)
                {
                    kickAnimationSpeed++;

                    if (kickAnimationSpeed == 3)
                    {
                        kickAnimationSpeed = 0;
                        kickBlueFrameNumber++;

                        if (kickBlueFrameNumber == 8)
                        {
                            kickBlueFrameNumber = 0;
                            kickAnimationSpeed = 0;
                        }
                    }
                }

                if (introTimer == 2300)
                {
                    frogBossPos.Y = playerPos.Y - 500;
                    frogBossPos.X = playerPos.X - 140;
                    frogBossFrameNumber = 0;

                    MediaPlayer.Stop();
                    musicToPlay = Content.Load<Song>("sound\\music\\frogBoss");
                    MediaPlayer.Play(musicToPlay);
                   
                }

                

                if (introTimer > 2300 && introTimer < 2580)
                {
                    if (frogBossPos.Y < (playerPos.Y - 200))
                    {
                        frogBossPos.Y += 10;
                    }
                }

                if (frogBossPos.Y >= playerPos.Y - 200)
                {
                    frogBossFrameNumber = 1;

                    
                }

                

                if (introTimer == 3660)
                {
                    levelNumber = 99;
                    previousLevelNumber = 1;

                    loadLevel("level" + levelNumber);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyPressed == false)
                {
                    keyPressed = true;
                    levelNumber = 99;
                    previousLevelNumber = 1;

                    loadLevel("level" + levelNumber);
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                {
                    keyPressed = false;
                }

            }

            //control ending
            if (levelNumber == 96)
            {
                scrolling();
                endingTimer++;



                //kickRedPos.X = 6440-offSet.X;
                //kickRedPos.Y = 1640 - offSet.Y;
                //kickBluePos.X = 6880 - offSet.X;
                //kickBluePos.Y = 1640 - offSet.Y;

                //introBallPos.X = 6780 - offSet.X;
                //introBallPos.Y = 1640 - offSet.Y;

                //kickRedPos.X = 6560;
                //kickRedPos.Y = 1640;
                //kickBluePos.X = 7000;
                //kickBluePos.Y = 1640;

                //introBallPos.X = 6780;
                //introBallPos.Y = 1648;

            }



        }

        private void doEnemys()
        {

            #region sheepBigs
            if (levelNumber == 6 || levelNumber == 14)
            {
                //animate sheepBigs
                sheepBigAnimationSpeed++;
                if (sheepBigAnimationSpeed == 8)
                {
                    sheepBigFrameNumber++;
                    sheepBigAnimationSpeed = 0;
                }

                if (sheepBigFrameNumber > 1)
                {
                    sheepBigFrameNumber = 0;
                }

                //sheepBigs hurt (do enemys)

                if (Collision(playerRect, sheepBigs[0].sheepBigHitRect) && gravity <= 0.0f
                    && sheepBigs[0].active == true && playerHurt == 0 && sheepBigs[0].hurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }





                //if the enemy is outside the play area, make it inactive
                if (sheepBigs[0].pos.X < 0 || sheepBigs[0].pos.X > 19900 ||
                    sheepBigs[0].pos.Y < 0 || sheepBigs[0].pos.Y > 5900)
                {
                    sheepBigs[0].active = false;
                }

                if (sheepBigs[0].active == true)
                {
                    if (sheepBigs[0].goingRight == true)
                    {
                        sheepBigs[0].pos.X += 2.0f;

                        if (level[(int)sheepBigs[0].pos.Y / 40, ((int)(sheepBigs[0].pos.X + sheepBigWidth + 3) / 40)] > 0
                        || level[(int)sheepBigs[0].pos.Y / 40, ((int)(sheepBigs[0].pos.X + sheepBigWidth + 3) / 40)] == -15)
                        {
                            sheepBigs[0].goingRight = false;
                        }
                    }
                    else
                    {
                        sheepBigs[0].pos.X -= 2.0f;

                        if (level[(int)sheepBigs[0].pos.Y / 40, ((int)(sheepBigs[0].pos.X - 3) / 40)] > 0
                        || level[(int)sheepBigs[0].pos.Y / 40, ((int)(sheepBigs[0].pos.X - 3) / 40)] == -15)
                        {
                            sheepBigs[0].goingRight = true;
                        }
                    }
                }



                //player jumps on sheepBig to hurt it
                if (Collision(playerRect, sheepBigs[0].sheepBigHitRect) && gravity > 0.0f && sheepBigs[0].active == true && sheepBigs[0].hurt == 0)
                {
                    playerHurt = -10;
                    if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
                    {
                        gravity = jumpHeight;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    {
                        gravity = jumpBoostOffEnemy;
                    }
                    //onGround = false;
                    //sounds.PlayCue("enemydie");
                    if (sheepBigs[0].energy > 0)
                    {
                        jumpOnBossSfx.Play(sfxVol);
                    }
                    if (sheepBigs[0].energy == 0)
                    {
                        bossDieSfx.Play(sfxVol);
                    }
                    sheepBigs[0].hurt = 240;
                    sheepBigs[0].energy--;

                    mrstickFrameNumber = 18;


                }

                if (sheepBigs[0].hurt > 0)
                {
                    sheepBigs[0].hurt--;
                }


                if (levelNumber == 6)
                {
                    if (sheepBigs[0].energy == 0)
                    {
                        sheepBigs[0].active = false;
                        sheepBigs[0].pos.X = -500;
                        sheepBigs[0].pos.Y = -500;
                        sheepBigs[0].energy = -10;
                        level[114, 77] = 0;
                        level[115, 77] = 0;
                        level[116, 77] = 0;
                        level[118, 74] = 46;
                        level[120, 72] = 46;
                        level[122, 70] = 46;
                    }
                }

            }




            #endregion

            #region duckBigs
            if (levelNumber == 10 || levelNumber == 14)
            {
                //animate duckBigs
                duckBigAnimationSpeed++;
                if (duckBigAnimationSpeed == 8)
                {
                    duckBigFrameNumber++;
                    duckBigAnimationSpeed = 0;
                }

                if (duckBigFrameNumber > 3)
                {
                    duckBigFrameNumber = 0;
                }

                //duckBigs hurt (do enemys)

                if (Collision(playerRect, duckBigs[0].duckBigHitRect) && gravity <= 0.0f
                    && duckBigs[0].active == true && playerHurt == 0 && duckBigs[0].hurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }





                //if the enemy is outside the play area, make it inactive
                if (duckBigs[0].pos.X < 0 || duckBigs[0].pos.X > 19900 ||
                    duckBigs[0].pos.Y < 0 || duckBigs[0].pos.Y > 5900)
                {
                    duckBigs[0].active = false;
                }

                if (duckBigs[0].active == true)
                {
                    if (duckBigs[0].goingRight == true)
                    {
                        duckBigs[0].pos.X += 2.0f;

                        if (level[(int)duckBigs[0].pos.Y / 40, ((int)(duckBigs[0].pos.X + duckBigWidth + 3) / 40)] > 0
                        || level[(int)duckBigs[0].pos.Y / 40, ((int)(duckBigs[0].pos.X + duckBigWidth + 3) / 40)] == -15)
                        {
                            duckBigs[0].goingRight = false;
                        }
                    }
                    else
                    {
                        duckBigs[0].pos.X -= 2.0f;

                        if (level[(int)duckBigs[0].pos.Y / 40, ((int)(duckBigs[0].pos.X - 3) / 40)] > 0
                        || level[(int)duckBigs[0].pos.Y / 40, ((int)(duckBigs[0].pos.X - 3) / 40)] == -15)
                        {
                            duckBigs[0].goingRight = true;
                        }
                    }
                }



                //player jumps on duckBig to hurt it
                if (Collision(playerRect, duckBigs[0].duckBigHitRect) && gravity > 0.0f && duckBigs[0].active == true && duckBigs[0].hurt == 0)
                {
                    playerHurt = -10;
                    if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
                    {
                        gravity = jumpHeight;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    {
                        gravity = jumpBoostOffEnemy;
                    }
                    //onGround = false;
                    //sounds.PlayCue("enemydie");
                    if (duckBigs[0].energy > 0)
                    {
                        jumpOnBossSfx.Play(sfxVol);
                    }
                    if (duckBigs[0].energy == 0)
                    {
                        bossDieSfx.Play(sfxVol);
                    }
                    duckBigs[0].hurt = 240;
                    duckBigs[0].energy--;

                    mrstickFrameNumber = 18;


                }

                if (duckBigs[0].hurt > 0)
                {
                    duckBigs[0].hurt--;
                }


                if (levelNumber == 10)
                {
                    if (duckBigs[0].energy == 0)
                    {
                        duckBigs[0].active = false;
                        duckBigs[0].pos.X = -500;
                        duckBigs[0].pos.Y = -500;
                        duckBigs[0].energy = -10;
                        level[123, 77] = 0;
                        level[124, 77] = 0;
                        level[125, 77] = 0;
                        level[112, 76] = 46;
                        level[117, 76] = 46;
                        level[122, 76] = 46;
                    }
                }

                if (levelNumber == 14)
                {
                    if (duckBigs[0].energy == 0)
                    {
                        duckBigs[0].active = false;
                        duckBigs[0].pos.X = -500;
                        duckBigs[0].pos.Y = -500;
                        duckBigs[0].energy = -10;

                    }

                    if (sheepBigs[0].energy == 0)
                    {
                        sheepBigs[0].active = false;
                        sheepBigs[0].pos.X = -500;
                        sheepBigs[0].pos.Y = -500;
                        sheepBigs[0].energy = -10;
                    }

                    if (duckBigs[0].active == false && sheepBigs[0].active == false)
                    {
                        #region delete blocks from level 14
                        level[116, 43] = 0;
                        level[117, 43] = 0;
                        level[118, 43] = 0;
                        level[116, 44] = 0;
                        level[118, 44] = 0;
                        level[116, 45] = 0;
                        level[117, 45] = 0;
                        level[118, 45] = 0;

                        level[116, 49] = 0;
                        level[117, 49] = 0;
                        level[118, 49] = 0;
                        level[116, 50] = 0;
                        level[118, 50] = 0;
                        level[116, 51] = 0;
                        level[117, 51] = 0;
                        level[118, 51] = 0;

                        level[116, 55] = 0;
                        level[117, 55] = 0;
                        level[118, 55] = 0;
                        level[116, 56] = 0;
                        level[118, 56] = 0;
                        level[116, 57] = 0;
                        level[117, 57] = 0;
                        level[118, 57] = 0;

                        level[116, 61] = 0;
                        level[117, 61] = 0;
                        level[118, 61] = 0;
                        level[116, 62] = 0;
                        level[118, 62] = 0;
                        level[116, 63] = 0;
                        level[117, 63] = 0;
                        level[118, 63] = 0;

                        level[116, 67] = 0;
                        level[117, 67] = 0;
                        level[118, 67] = 0;
                        level[116, 68] = 0;
                        level[118, 68] = 0;
                        level[116, 69] = 0;
                        level[117, 69] = 0;
                        level[118, 69] = 0;

                        level[116, 73] = 0;
                        level[117, 73] = 0;
                        level[118, 73] = 0;
                        level[116, 74] = 0;
                        level[118, 74] = 0;
                        level[116, 75] = 0;
                        level[117, 75] = 0;
                        level[118, 75] = 0;

                        level[116, 92] = 0;
                        level[117, 92] = 0;
                        level[118, 92] = 0;
                        level[116, 93] = 0;
                        level[118, 93] = 0;
                        level[116, 94] = 0;
                        level[117, 94] = 0;
                        level[118, 94] = 0;

                        level[116, 98] = 0;
                        level[117, 98] = 0;
                        level[118, 98] = 0;
                        level[116, 99] = 0;
                        level[118, 99] = 0;
                        level[116, 100] = 0;
                        level[117, 100] = 0;
                        level[118, 100] = 0;

                        level[116, 104] = 0;
                        level[117, 104] = 0;
                        level[118, 104] = 0;
                        level[116, 105] = 0;
                        level[118, 105] = 0;
                        level[116, 106] = 0;
                        level[117, 106] = 0;
                        level[118, 106] = 0;

                        level[116, 109] = 0;
                        level[117, 109] = 0;
                        level[118, 109] = 0;
                        level[116, 110] = 0;
                        level[118, 110] = 0;
                        level[116, 111] = 0;
                        level[117, 111] = 0;
                        level[118, 111] = 0;

                        level[118, 79] = 0;
                        level[118, 82] = 0;
                        level[118, 85] = 0;
                        level[118, 88] = 0;

                        level[126, 80] = 0;
                        level[126, 81] = 0;
                        level[126, 82] = 0;
                        level[126, 83] = 0;
                        level[126, 84] = 0;
                        level[126, 85] = 0;
                        level[126, 86] = 0;
                        level[126, 87] = 0;
                        #endregion
                    }


                }

            }




            #endregion

            #region control frog boss level
            if (levelNumber == 19)
            {
                //frogBoss hurt (do enemys)

                if (Collision(playerRect, frogBossHitRect) && gravity <= 0.0f
                    && frogBossActive == true && playerHurt == 0 && frogBossHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }

                //player jumps on frogBoss to hurt it
                if (Collision(playerRect, frogBossHitRect) && gravity > 0.0f && frogBossActive == true && frogBossHurt == 0)
                {
                    playerHurt = -10;
                    if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
                    {
                        gravity = jumpHeight;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    {
                        gravity = jumpBoostOffEnemy;
                    }
                    //onGround = false;
                    //sounds.PlayCue("enemydie");
                    frogBossHurt = 240;
                    frogBossEnergy--;

                    jumpOnBossSfx.Play(sfxVol);

                    mrstickFrameNumber = 18;


                }

                if (frogBossEnergy == 0)
                {
                    frogBossEnergy = -400;
                    bossDieSfx.Play(sfxVol);
                }

                if (frogBossEnergy == -400)
                {
                    frogBossActive = false;
                    frogBossPos.X = -500;
                    frogBossPos.Y = -500;
                }

                if (frogBossEnergy < 0)
                {
                    frogBossEnergy++;
                }

                if (frogBossEnergy == -10)
                {
                    levelNumber = 96;
                    previousLevelNumber = 1;

                    loadLevel("level" + levelNumber);
                }



                if (frogBossHurt > 0)
                {
                    frogBossHurt--;
                }

                //frog boss action script
                if (levelNumber == 19)
                {
                    if (playerPos.X < 2960)
                    {
                        frogFlameActive = false;
                    }

                    if (frogFlameActive == true)
                    {
                        frogFlamePos.Y += 7;

                        frogFlameAnimationSpeed++;
                        if (frogFlameAnimationSpeed == 6)
                        {
                            frogFlameFrameNumber++;
                            frogFlameAnimationSpeed = 0;
                        }

                        if (frogFlameFrameNumber > 1)
                        {
                            frogFlameFrameNumber = 0;
                        }
                    }

                    if (frogFlamePos.Y >= 2720)
                    {
                        frogFlameActive = false;
                        flameSfx.Play(sfxVol);
                    }

                    if (frogFlameActive == false)
                    {
                        frogFlamePos.X = -500;
                        frogFlamePos.Y = -500;
                    }


                    //players position reaches a certain point, start the script timer
                    if (playerPos.X > 2960)
                    {
                        frogBossTimer++;
                    }


                    //first jump of the frog boss
                    if (frogBossEnergy > 0)
                    {


                        if (frogBossTimer == 300)
                        {
                            frogBossJumpSfx.Play(sfxVol);
                        }

                        if (frogBossTimer > 300 && frogBossTimer < 500)
                        {
                            frogBossPos.Y -= 10;
                            frogBossFrameNumber = 0;



                            if (frogBossTimer == 350)
                            {
                                frogFlameActive = true;
                                frogFlamePos.X = 3860;
                                frogFlamePos.Y = 2000;
                                lightningSfx.Play(sfxVol);
                            }

                            if (frogBossTimer == 450)
                            {

                                frogFlamePos.X = 3260;
                                frogFlamePos.Y = 2000;

                                frogFlameActive = true;
                                lightningSfx.Play(sfxVol);
                            }

                        }



                        //move the frog to its left attacking side
                        if (frogBossTimer == 500)
                        {
                            frogBossPos.X = 2960;
                        }


                        //move it down to the ground
                        if (frogBossTimer > 500 && frogBossTimer < 700)
                        {
                            frogBossPos.Y += 10;
                        }

                        if (frogBossTimer == 700)
                        {
                            frogBossFrameNumber = 1;
                            frogBossLandSfx.Play(sfxVol);


                        }


                        //frog jumps into the air destined for its tight hand side attacking spot

                        if (frogBossTimer == 1000)
                        {
                            frogBossJumpSfx.Play(sfxVol);
                        }

                        if (frogBossTimer > 1000 && frogBossTimer < 1200)
                        {
                            frogBossPos.Y -= 10;
                            frogBossFrameNumber = 0;

                            if (frogBossTimer == 1050)
                            {
                                frogFlameActive = true;
                                frogFlamePos.X = 3060;
                                frogFlamePos.Y = 2000;

                                lightningSfx.Play(sfxVol);
                            }

                            if (frogBossTimer == 1150)
                            {

                                frogFlamePos.X = 3560;
                                frogFlamePos.Y = 2000;

                                frogFlameActive = true;

                                lightningSfx.Play(sfxVol);
                            }



                        }


                        //fire a further fireball
                        if (frogBossTimer == 1250)
                        {

                            frogFlamePos.X = 4060;
                            frogFlamePos.Y = 2000;

                            frogFlameActive = true;

                            lightningSfx.Play(sfxVol);
                        }


                        //move the frog to its right most attack area
                        if (frogBossTimer == 1200)
                        {
                            frogBossPos.X = 3978;
                        }

                        //move it down to the ground
                        if (frogBossTimer > 1200 && frogBossTimer < 1400)
                        {
                            frogBossPos.Y += 10;

                        }


                        //land on the right hand side
                        if (frogBossTimer == 1400)
                        {
                            frogBossFrameNumber = 1;
                            frogBossTimer = 0;

                            frogBossLandSfx.Play(sfxVol);


                        }

                    }

                }

                //frogFlames hurt
                if (Collision(playerRect, frogFlameHitRect) && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;

                    ouchSfx.Play();

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }

            }

            #endregion


            #region bees

            //animate bees
            beeAnimationSpeed++;
            if (beeAnimationSpeed == 2)
            {
                beeFrameNumber++;
                beeAnimationSpeed = 0;
            }

            if (beeFrameNumber > 1)
            {
                beeFrameNumber = 0;
            }

            //bees hurt (do enemys)

            for (int i = 0; i < 10; i++)
            {
                if (Collision(playerRect, bees[i].beeHitRect) && gravity <= 0.0f && bees[i].active == true && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }





                //if the enemy is outside the play area, make it inactive
                if (bees[i].pos.X < 0 || bees[i].pos.X > 19900 ||
                    bees[i].pos.Y < 0 || bees[i].pos.Y > 5900)
                {
                    bees[i].active = false;
                }

                if (bees[i].active == true)
                {
                    if (bees[i].goingRight == true)
                    {
                        bees[i].pos.X += 1.0f;

                        if (level[(int)bees[i].pos.Y / 40, ((int)(bees[i].pos.X + beeWidth) / 40)] > 0
                        || level[(int)bees[i].pos.Y / 40, ((int)(bees[i].pos.X + beeWidth) / 40)] == -15
                            || level[(int)bees[i].pos.Y / 40, ((int)(bees[i].pos.X + beeWidth) / 40)] == -346
                            || level[(int)bees[i].pos.Y / 40, ((int)(bees[i].pos.X + beeWidth) / 40)] == -347
                            || level[(int)bees[i].pos.Y / 40, ((int)(bees[i].pos.X + beeWidth) / 40)] == -348
                            || level[(int)bees[i].pos.Y / 40, ((int)(bees[i].pos.X + beeWidth) / 40)] == -349)
                        {
                            bees[i].goingRight = false;
                        }
                    }
                    else
                    {
                        bees[i].pos.X -= 1.0f;

                        if (level[(int)bees[i].pos.Y / 40, ((int)(bees[i].pos.X - 1) / 40)] > 0
                        || level[(int)bees[i].pos.Y / 40, ((int)(bees[i].pos.X - 1) / 40)] == -15
                            || level[(int)bees[i].pos.Y / 40, ((int)(bees[i].pos.X - 1) / 40)] == -346
                            || level[(int)bees[i].pos.Y / 40, ((int)(bees[i].pos.X - 1) / 40)] == -347
                            || level[(int)bees[i].pos.Y / 40, ((int)(bees[i].pos.X - 1) / 40)] == -348
                            || level[(int)bees[i].pos.Y / 40, ((int)(bees[i].pos.X - 1) / 40)] == -349)
                        {
                            bees[i].goingRight = true;
                        }
                    }
                }

                //make the enemy bob up and down as it flys

                bees[i].heightModifier--;

                if (bees[i].heightModifier == 0)
                {
                    bees[i].heightModifier = 240;
                }

                if (bees[i].heightModifier > 120)
                {
                    bees[i].pos.Y -= 0.25f;
                }

                if (bees[i].heightModifier < 120)
                {
                    bees[i].pos.Y += 0.25f;
                }



                //player jumps on bee to hurt it
                if (Collision(playerRect, bees[i].beeHitRect) && gravity > 0.0f && bees[i].active == true)
                {
                    playerHurt = -10;
                    bees[i].active = false;
                    if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
                    {
                        gravity = jumpHeight;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    {
                        gravity = jumpBoostOffEnemy;
                    }
                    //onGround = false;
                    //sounds.PlayCue("enemydie");
                    jumpOnEnemySfx.Play(sfxVol);
                    bees[i].pos.X = -100;
                    bees[i].pos.Y = -100;
                    score += 100;
                    mrstickFrameNumber = 18;



                }
            }






            #endregion

            #region birds

            //animate birds
            birdAnimationSpeed++;
            if (birdAnimationSpeed == 8)
            {
                birdFrameNumber++;
                birdAnimationSpeed = 0;
            }

            if (birdFrameNumber > 3)
            {
                birdFrameNumber = 0;
            }

            //birds hurt (do enemys)

            for (int i = 0; i < 10; i++)
            {
                if (Collision(playerRect, birds[i].birdHitRect) && gravity <= 0.0f && birds[i].active == true && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }





                //if the enemy is outside the play area, make it inactive
                if (birds[i].pos.X < 0 || birds[i].pos.X > 19900 ||
                    birds[i].pos.Y < 0 || birds[i].pos.Y > 5900)
                {
                    birds[i].active = false;
                }

                if (birds[i].active == true)
                {
                    if (birds[i].goingRight == true)
                    {
                        birds[i].pos.X += 1.0f;

                        if (level[(int)birds[i].pos.Y / 40, ((int)(birds[i].pos.X + birdWidth) / 40)] > 0
                        || level[(int)birds[i].pos.Y / 40, ((int)(birds[i].pos.X + birdWidth) / 40)] == -15
                            || level[(int)birds[i].pos.Y / 40, ((int)(birds[i].pos.X + birdWidth) / 40)] == -346
                            || level[(int)birds[i].pos.Y / 40, ((int)(birds[i].pos.X + birdWidth) / 40)] == -347
                            || level[(int)birds[i].pos.Y / 40, ((int)(birds[i].pos.X + birdWidth) / 40)] == -348
                            || level[(int)birds[i].pos.Y / 40, ((int)(birds[i].pos.X + birdWidth) / 40)] == -349)
                        {
                            birds[i].goingRight = false;
                        }
                    }
                    else
                    {
                        birds[i].pos.X -= 1.0f;

                        if (level[(int)birds[i].pos.Y / 40, ((int)(birds[i].pos.X - 1) / 40)] > 0
                        || level[(int)birds[i].pos.Y / 40, ((int)(birds[i].pos.X - 1) / 40)] == -15
                            || level[(int)birds[i].pos.Y / 40, ((int)(birds[i].pos.X - 1) / 40)] == -346
                            || level[(int)birds[i].pos.Y / 40, ((int)(birds[i].pos.X - 1) / 40)] == -347
                            || level[(int)birds[i].pos.Y / 40, ((int)(birds[i].pos.X - 1) / 40)] == -348
                            || level[(int)birds[i].pos.Y / 40, ((int)(birds[i].pos.X - 1) / 40)] == -349)
                        {
                            birds[i].goingRight = true;
                        }
                    }
                }

                //make the enemy bob up and down as it flys

                birds[i].heightModifier--;

                if (birds[i].heightModifier == 0)
                {
                    birds[i].heightModifier = 240;
                }

                if (birds[i].heightModifier > 120)
                {
                    birds[i].pos.Y -= 0.25f;
                }

                if (birds[i].heightModifier < 120)
                {
                    birds[i].pos.Y += 0.25f;
                }



                //player jumps on bird to hurt it
                if (Collision(playerRect, birds[i].birdHitRect) && gravity > 0.0f && birds[i].active == true)
                {
                    playerHurt = -10;
                    birds[i].active = false;
                    if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
                    {
                        gravity = jumpHeight;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    {
                        gravity = jumpBoostOffEnemy;
                    }
                    //onGround = false;
                    //sounds.PlayCue("enemydie");
                    jumpOnEnemySfx.Play(sfxVol);
                    birds[i].pos.X = -100;
                    birds[i].pos.Y = -100;
                    score += 100;
                    mrstickFrameNumber = 18;



                }
            }






            #endregion

            #region ducks

            //animate ducks
            duckAnimationSpeed++;
            if (duckAnimationSpeed == 8)
            {
                duckFrameNumber++;
                duckAnimationSpeed = 0;
            }

            if (duckFrameNumber > 3)
            {
                duckFrameNumber = 0;
            }

            //ducks hurt (do enemys)

            for (int i = 0; i < 10; i++)
            {
                if (Collision(playerRect, ducks[i].duckHitRect) && gravity <= 0.0f && ducks[i].active == true && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }





                //if the enemy is outside the play area, make it inactive
                if (ducks[i].pos.X < 0 || ducks[i].pos.X > 19900 ||
                    ducks[i].pos.Y < 0 || ducks[i].pos.Y > 5900)
                {
                    ducks[i].active = false;
                }

                if (ducks[i].active == true)
                {
                    if (ducks[i].goingRight == true)
                    {
                        ducks[i].pos.X += 1.0f;

                        if (level[(int)ducks[i].pos.Y / 40, ((int)(ducks[i].pos.X + duckWidth) / 40)] > 0
                        || level[(int)ducks[i].pos.Y / 40, ((int)(ducks[i].pos.X + duckWidth) / 40)] == -15
                            || level[(int)ducks[i].pos.Y / 40, ((int)(ducks[i].pos.X + duckWidth) / 40)] == -346
                            || level[(int)ducks[i].pos.Y / 40, ((int)(ducks[i].pos.X + duckWidth) / 40)] == -347
                            || level[(int)ducks[i].pos.Y / 40, ((int)(ducks[i].pos.X + duckWidth) / 40)] == -348
                            || level[(int)ducks[i].pos.Y / 40, ((int)(ducks[i].pos.X + duckWidth) / 40)] == -349)
                        {
                            ducks[i].goingRight = false;
                        }
                    }
                    else
                    {
                        ducks[i].pos.X -= 1.0f;

                        if (level[(int)ducks[i].pos.Y / 40, ((int)(ducks[i].pos.X - 1) / 40)] > 0
                        || level[(int)ducks[i].pos.Y / 40, ((int)(ducks[i].pos.X - 1) / 40)] == -15
                            || level[(int)ducks[i].pos.Y / 40, ((int)(ducks[i].pos.X - 1) / 40)] == -346
                            || level[(int)ducks[i].pos.Y / 40, ((int)(ducks[i].pos.X - 1) / 40)] == -347
                            || level[(int)ducks[i].pos.Y / 40, ((int)(ducks[i].pos.X - 1) / 40)] == -348
                            || level[(int)ducks[i].pos.Y / 40, ((int)(ducks[i].pos.X - 1) / 40)] == -349)
                        {
                            ducks[i].goingRight = true;
                        }
                    }
                }



                //player jumps on duck to hurt it
                if (Collision(playerRect, ducks[i].duckHitRect) && gravity > 0.0f && ducks[i].active == true)
                {
                    playerHurt = -10;
                    ducks[i].active = false;
                    if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
                    {
                        gravity = jumpHeight;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    {
                        gravity = jumpBoostOffEnemy;
                    }
                    //onGround = false;
                    //sounds.PlayCue("enemydie");
                    jumpOnEnemySfx.Play(sfxVol);
                    ducks[i].pos.X = -100;
                    ducks[i].pos.Y = -100;
                    score += 100;
                    mrstickFrameNumber = 18;


                }
            }






            #endregion

            #region fishs

            //animate fishs
            fishAnimationSpeed++;
            if (fishAnimationSpeed == 8)
            {
                fishFrameNumber++;
                fishAnimationSpeed = 0;
            }

            if (fishFrameNumber > 1)
            {
                fishFrameNumber = 0;
            }

            //fishs hurt (do enemys)

            for (int i = 0; i < 10; i++)
            {
                if (Collision(playerRect, fishs[i].fishHitRect) && gravity <= 0.0f && fishs[i].active == true && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }





                //if the enemy is outside the play area, make it inactive
                if (fishs[i].pos.X < 0 || fishs[i].pos.X > 19900 ||
                    fishs[i].pos.Y < 0 || fishs[i].pos.Y > 5900)
                {
                    fishs[i].active = false;
                }

                if (fishs[i].active == true)
                {
                    if (fishs[i].goingUp == true)
                    {
                        fishs[i].pos.Y -= 3.0f;

                        if (level[(int)fishs[i].pos.Y / 40, ((int)(fishs[i].pos.X) / 40)] == -15
                            || level[(int)fishs[i].pos.Y / 40, ((int)(fishs[i].pos.X) / 40)] > 0)
                        {
                            fishs[i].goingUp = false;
                        }
                    }
                    else
                    {
                        fishs[i].pos.Y += 3.0f;

                        if (level[(int)((fishs[i].pos.Y)) / 40, ((int)(fishs[i].pos.X - 1) / 40)] == -15
                            || level[(int)((fishs[i].pos.Y)) / 40, ((int)(fishs[i].pos.X - 1) / 40)] > 0)
                        {
                            fishs[i].goingUp = true;
                        }
                    }
                }


            }






            #endregion

            #region hedgehogs

            //animate hedgehogs
            hedgehogAnimationSpeed++;
            if (hedgehogAnimationSpeed == 8)
            {
                hedgehogFrameNumber++;
                hedgehogAnimationSpeed = 0;
            }

            if (hedgehogFrameNumber > 1)
            {
                hedgehogFrameNumber = 0;
            }

            //hedgehogs hurt (do enemys)

            for (int i = 0; i < 10; i++)
            {
                if (Collision(playerRect, hedgehogs[i].hedgehogHitRect) && gravity <= 0.0f && hedgehogs[i].active == true && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }





                //if the enemy is outside the play area, make it inactive
                if (hedgehogs[i].pos.X < 0 || hedgehogs[i].pos.X > 19900 ||
                    hedgehogs[i].pos.Y < 0 || hedgehogs[i].pos.Y > 5900)
                {
                    hedgehogs[i].active = false;
                }

                if (hedgehogs[i].active == true)
                {
                    if (hedgehogs[i].goingRight == true)
                    {
                        hedgehogs[i].pos.X += 1.0f;

                        if (level[(int)hedgehogs[i].pos.Y / 40, ((int)(hedgehogs[i].pos.X + hedgehogWidth) / 40)] > 0
                        || level[(int)hedgehogs[i].pos.Y / 40, ((int)(hedgehogs[i].pos.X + hedgehogWidth) / 40)] == -15
                            || level[(int)hedgehogs[i].pos.Y / 40, ((int)(hedgehogs[i].pos.X + hedgehogWidth) / 40)] == -346
                            || level[(int)hedgehogs[i].pos.Y / 40, ((int)(hedgehogs[i].pos.X + hedgehogWidth) / 40)] == -347
                            || level[(int)hedgehogs[i].pos.Y / 40, ((int)(hedgehogs[i].pos.X + hedgehogWidth) / 40)] == -348
                            || level[(int)hedgehogs[i].pos.Y / 40, ((int)(hedgehogs[i].pos.X + hedgehogWidth) / 40)] == -349)
                        {
                            hedgehogs[i].goingRight = false;
                        }
                    }
                    else
                    {
                        hedgehogs[i].pos.X -= 1.0f;

                        if (level[(int)hedgehogs[i].pos.Y / 40, ((int)(hedgehogs[i].pos.X - 1) / 40)] > 0
                        || level[(int)hedgehogs[i].pos.Y / 40, ((int)(hedgehogs[i].pos.X - 1) / 40)] == -15
                            || level[(int)hedgehogs[i].pos.Y / 40, ((int)(hedgehogs[i].pos.X - 1) / 40)] == -346
                            || level[(int)hedgehogs[i].pos.Y / 40, ((int)(hedgehogs[i].pos.X - 1) / 40)] == -347
                            || level[(int)hedgehogs[i].pos.Y / 40, ((int)(hedgehogs[i].pos.X - 1) / 40)] == -348
                            || level[(int)hedgehogs[i].pos.Y / 40, ((int)(hedgehogs[i].pos.X - 1) / 40)] == -349)
                        {
                            hedgehogs[i].goingRight = true;
                        }
                    }
                }


            }






            #endregion

            #region ladybirds

            //animate ladybirds
            ladybirdAnimationSpeed++;
            if (ladybirdAnimationSpeed == 8)
            {
                ladybirdFrameNumber++;
                ladybirdAnimationSpeed = 0;
            }

            if (ladybirdFrameNumber > 1)
            {
                ladybirdFrameNumber = 0;
            }

            //ladybirds hurt (do enemys)

            for (int i = 0; i < 10; i++)
            {
                if (Collision(playerRect, ladybirds[i].ladybirdHitRect) && gravity <= 0.0f && ladybirds[i].active == true && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }





                //if the enemy is outside the play area, make it inactive
                if (ladybirds[i].pos.X < 0 || ladybirds[i].pos.X > 19900 ||
                    ladybirds[i].pos.Y < 0 || ladybirds[i].pos.Y > 5900)
                {
                    ladybirds[i].active = false;
                }

                if (ladybirds[i].active == true)
                {
                    if (ladybirds[i].goingRight == true)
                    {
                        ladybirds[i].pos.X += 1.0f;

                        if (level[(int)ladybirds[i].pos.Y / 40, ((int)(ladybirds[i].pos.X + ladybirdWidth) / 40)] > 0
                        || level[(int)ladybirds[i].pos.Y / 40, ((int)(ladybirds[i].pos.X + ladybirdWidth) / 40)] == -15
                            || level[(int)ladybirds[i].pos.Y / 40, ((int)(ladybirds[i].pos.X + ladybirdWidth) / 40)] == -346
                            || level[(int)ladybirds[i].pos.Y / 40, ((int)(ladybirds[i].pos.X + ladybirdWidth) / 40)] == -347
                            || level[(int)ladybirds[i].pos.Y / 40, ((int)(ladybirds[i].pos.X + ladybirdWidth) / 40)] == -348
                            || level[(int)ladybirds[i].pos.Y / 40, ((int)(ladybirds[i].pos.X + ladybirdWidth) / 40)] == -349)
                        {
                            ladybirds[i].goingRight = false;
                        }
                    }
                    else
                    {
                        ladybirds[i].pos.X -= 1.0f;

                        if (level[(int)ladybirds[i].pos.Y / 40, ((int)(ladybirds[i].pos.X - 1) / 40)] > 0
                        || level[(int)ladybirds[i].pos.Y / 40, ((int)(ladybirds[i].pos.X - 1) / 40)] == -15
                            || level[(int)ladybirds[i].pos.Y / 40, ((int)(ladybirds[i].pos.X - 1) / 40)] == -346
                            || level[(int)ladybirds[i].pos.Y / 40, ((int)(ladybirds[i].pos.X - 1) / 40)] == -347
                            || level[(int)ladybirds[i].pos.Y / 40, ((int)(ladybirds[i].pos.X - 1) / 40)] == -348
                            || level[(int)ladybirds[i].pos.Y / 40, ((int)(ladybirds[i].pos.X - 1) / 40)] == -349)
                        {
                            ladybirds[i].goingRight = true;
                        }
                    }
                }



                //player jumps on ladybird to hurt it
                if (Collision(playerRect, ladybirds[i].jumpOnLadybirdRect) && gravity > 0.0f && ladybirds[i].active == true)
                {
                    playerHurt = -10;
                    ladybirds[i].active = false;


                    if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
                    {
                        gravity = jumpHeight;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    {
                        gravity = jumpBoostOffEnemy;
                    }

                    //onGround = false;
                    //sounds.PlayCue("enemydie");
                    jumpOnEnemySfx.Play(sfxVol);
                    ladybirds[i].pos.X = -100;
                    ladybirds[i].pos.Y = -100;
                    score += 100;
                    mrstickFrameNumber = 18;


                }
            }






            #endregion

            #region sheeps

            //animate sheeps
            sheepAnimationSpeed++;
            if (sheepAnimationSpeed == 8)
            {
                sheepFrameNumber++;
                sheepAnimationSpeed = 0;
            }

            if (sheepFrameNumber > 1)
            {
                sheepFrameNumber = 0;
            }

            //sheeps hurt (do enemys)

            for (int i = 0; i < 10; i++)
            {
                if (Collision(playerRect, sheeps[i].sheepHitRect) && gravity <= 0.0f && sheeps[i].active == true && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }





                //if the enemy is outside the play area, make it inactive
                if (sheeps[i].pos.X < 0 || sheeps[i].pos.X > 19900 ||
                    sheeps[i].pos.Y < 0 || sheeps[i].pos.Y > 5900)
                {
                    sheeps[i].active = false;
                }

                if (sheeps[i].active == true)
                {
                    if (sheeps[i].goingRight == true)
                    {
                        sheeps[i].pos.X += 1.0f;

                        if (level[(int)sheeps[i].pos.Y / 40, ((int)(sheeps[i].pos.X + sheepWidth) / 40)] > 0
                        || level[(int)sheeps[i].pos.Y / 40, ((int)(sheeps[i].pos.X + sheepWidth) / 40)] == -15
                            || level[(int)sheeps[i].pos.Y / 40, ((int)(sheeps[i].pos.X + sheepWidth) / 40)] == -346
                            || level[(int)sheeps[i].pos.Y / 40, ((int)(sheeps[i].pos.X + sheepWidth) / 40)] == -347
                            || level[(int)sheeps[i].pos.Y / 40, ((int)(sheeps[i].pos.X + sheepWidth) / 40)] == -348
                            || level[(int)sheeps[i].pos.Y / 40, ((int)(sheeps[i].pos.X + sheepWidth) / 40)] == -349)
                        {
                            sheeps[i].goingRight = false;
                        }
                    }
                    else
                    {
                        sheeps[i].pos.X -= 1.0f;

                        if (level[(int)sheeps[i].pos.Y / 40, ((int)(sheeps[i].pos.X - 1) / 40)] > 0
                        || level[(int)sheeps[i].pos.Y / 40, ((int)(sheeps[i].pos.X - 1) / 40)] == -15
                            || level[(int)sheeps[i].pos.Y / 40, ((int)(sheeps[i].pos.X - 1) / 40)] == -346
                            || level[(int)sheeps[i].pos.Y / 40, ((int)(sheeps[i].pos.X - 1) / 40)] == -347
                            || level[(int)sheeps[i].pos.Y / 40, ((int)(sheeps[i].pos.X - 1) / 40)] == -348
                            || level[(int)sheeps[i].pos.Y / 40, ((int)(sheeps[i].pos.X - 1) / 40)] == -349)
                        {
                            sheeps[i].goingRight = true;
                        }
                    }
                }



                //player jumps on sheep to hurt it
                if (Collision(playerRect, sheeps[i].sheepHitRect) && gravity > 0.0f && sheeps[i].active == true)
                {
                    playerHurt = -10;
                    sheeps[i].active = false;
                    if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
                    {
                        gravity = jumpHeight;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    {
                        gravity = jumpBoostOffEnemy;
                    }
                    //onGround = false;
                    //sounds.PlayCue("enemydie");
                    jumpOnEnemySfx.Play(sfxVol);
                    sheeps[i].pos.X = -100;
                    sheeps[i].pos.Y = -100;
                    score += 100;
                    mrstickFrameNumber = 18;


                }
            }






            #endregion

            #region snails

            //animate snails
            snailAnimationSpeed++;
            if (snailAnimationSpeed == 8)
            {
                snailFrameNumber++;
                snailAnimationSpeed = 0;
            }

            if (snailFrameNumber > 3)
            {
                snailFrameNumber = 0;
            }

            //snails hurt (do enemys)

            for (int i = 0; i < 10; i++)
            {
                if (Collision(playerRect, snails[i].snailHitRect) && gravity <= 0.0f && snails[i].active == true && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }





                //if the enemy is outside the play area, make it inactive
                if (snails[i].pos.X < 0 || snails[i].pos.X > 19900 ||
                    snails[i].pos.Y < 0 || snails[i].pos.Y > 5900)
                {
                    snails[i].active = false;
                }

                if (snails[i].active == true)
                {
                    if (snails[i].goingRight == true)
                    {
                        snails[i].pos.X += 1.0f;

                        if (level[(int)snails[i].pos.Y / 40, ((int)(snails[i].pos.X + snailWidth) / 40)] > 0
                        || level[(int)snails[i].pos.Y / 40, ((int)(snails[i].pos.X + snailWidth) / 40)] == -15
                            || level[(int)snails[i].pos.Y / 40, ((int)(snails[i].pos.X + snailWidth) / 40)] == -346
                            || level[(int)snails[i].pos.Y / 40, ((int)(snails[i].pos.X + snailWidth) / 40)] == -347
                            || level[(int)snails[i].pos.Y / 40, ((int)(snails[i].pos.X + snailWidth) / 40)] == -348
                            || level[(int)snails[i].pos.Y / 40, ((int)(snails[i].pos.X + snailWidth) / 40)] == -349)
                        {
                            snails[i].goingRight = false;
                        }
                    }
                    else
                    {
                        snails[i].pos.X -= 1.0f;

                        if (level[(int)snails[i].pos.Y / 40, ((int)(snails[i].pos.X - 1) / 40)] > 0
                        || level[(int)snails[i].pos.Y / 40, ((int)(snails[i].pos.X - 1) / 40)] == -15
                            || level[(int)snails[i].pos.Y / 40, ((int)(snails[i].pos.X - 1) / 40)] == -346
                            || level[(int)snails[i].pos.Y / 40, ((int)(snails[i].pos.X - 1) / 40)] == -347
                            || level[(int)snails[i].pos.Y / 40, ((int)(snails[i].pos.X - 1) / 40)] == -348
                            || level[(int)snails[i].pos.Y / 40, ((int)(snails[i].pos.X - 1) / 40)] == -349)
                        {
                            snails[i].goingRight = true;
                        }
                    }
                }



                //player jumps on snail to hurt it
                if (Collision(playerRect, snails[i].snailHitRect) && gravity > 0.0f && snails[i].active == true)
                {
                    playerHurt = -10;
                    snails[i].active = false;
                    if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
                    {
                        gravity = jumpHeight;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    {
                        gravity = jumpBoostOffEnemy;
                    }
                    //onGround = false;
                    //sounds.PlayCue("enemydie");
                    jumpOnEnemySfx.Play(sfxVol);
                    snails[i].pos.X = -100;
                    snails[i].pos.Y = -100;
                    score += 100;
                    mrstickFrameNumber = 18;


                }
            }






            #endregion

            #region turtles

            //animate turtles
            turtleAnimationSpeed++;
            if (turtleAnimationSpeed == 8)
            {
                turtleFrameNumber++;
                turtleAnimationSpeed = 0;
            }

            if (turtleFrameNumber > 2)
            {
                turtleFrameNumber = 0;
            }

            //turtles hurt (do enemys)

            for (int i = 0; i < 10; i++)
            {
                if (Collision(playerRect, turtles[i].turtleRect) && gravity <= 0.0f && turtles[i].active == true && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }








                //if the enemy is outside the play area, make it inactive
                if (turtles[i].pos.X < 0 || turtles[i].pos.X > 19900 ||
                    turtles[i].pos.Y < 0 || turtles[i].pos.Y > 5900)
                {
                    turtles[i].active = false;
                }

                if (turtles[i].active == true)
                {
                    if (turtles[i].goingRight == true)
                    {
                        turtles[i].pos.X += 1.0f;

                        if (level[(int)turtles[i].pos.Y / 40, ((int)(turtles[i].pos.X + turtleWidth) / 40)] > 0
                        || level[(int)turtles[i].pos.Y / 40, ((int)(turtles[i].pos.X + turtleWidth) / 40)] == -15
                            || level[(int)turtles[i].pos.Y / 40, ((int)(turtles[i].pos.X + turtleWidth) / 40)] == -346
                            || level[(int)turtles[i].pos.Y / 40, ((int)(turtles[i].pos.X + turtleWidth) / 40)] == -347
                            || level[(int)turtles[i].pos.Y / 40, ((int)(turtles[i].pos.X + turtleWidth) / 40)] == -348
                            || level[(int)turtles[i].pos.Y / 40, ((int)(turtles[i].pos.X + turtleWidth) / 40)] == -349)
                        {
                            turtles[i].goingRight = false;
                        }
                    }
                    else
                    {
                        turtles[i].pos.X -= 1.0f;

                        if (level[(int)turtles[i].pos.Y / 40, ((int)(turtles[i].pos.X - 1) / 40)] > 0
                        || level[(int)turtles[i].pos.Y / 40, ((int)(turtles[i].pos.X - 1) / 40)] == -15
                            || level[(int)turtles[i].pos.Y / 40, ((int)(turtles[i].pos.X - 1) / 40)] == -346
                            || level[(int)turtles[i].pos.Y / 40, ((int)(turtles[i].pos.X - 1) / 40)] == -347
                            || level[(int)turtles[i].pos.Y / 40, ((int)(turtles[i].pos.X - 1) / 40)] == -348
                            || level[(int)turtles[i].pos.Y / 40, ((int)(turtles[i].pos.X - 1) / 40)] == -349)
                        {
                            turtles[i].goingRight = true;
                        }
                    }
                }



                //player jumps on turtle to hurt it
                if (Collision(playerRect, turtles[i].jumpOnTurtleRect) && gravity > 0.0f && turtles[i].active == true)
                {
                    playerHurt = -10;
                    turtles[i].active = false;
                    if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
                    {
                        gravity = jumpHeight;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                    {
                        gravity = jumpBoostOffEnemy;
                    }
                    //onGround = false;
                    jumpOnEnemySfx.Play(sfxVol);
                    turtles[i].pos.X = -100;
                    turtles[i].pos.Y = -100;
                    score += 100;
                    mrstickFrameNumber = 18;


                }
            }






            #endregion



            #region springMushrooms




            //spring mushrooms

            for (int i = 0; i < 10; i++)
            {





                //if the spring is outside the play area, make it inactive
                if (springMushrooms[i].pos.X < 0 || springMushrooms[i].pos.X > 19900 ||
                    springMushrooms[i].pos.Y < 0 || springMushrooms[i].pos.Y > 5900)
                {
                    springMushrooms[i].active = false;
                }

                //control the springs animation
                if (springMushrooms[i].animationSpeed > 0)
                {
                    springMushrooms[i].animationSpeed--;

                    if (springMushrooms[i].animationSpeed >= 10)
                    {
                        springMushrooms[i].frameNumber = 1;
                    }

                    if (springMushrooms[i].animationSpeed >= 5 && springMushrooms[i].animationSpeed < 10)
                    {
                        springMushrooms[i].frameNumber = 2;
                    }

                    if (springMushrooms[i].animationSpeed < 5 && springMushrooms[i].animationSpeed > 0)
                    {
                        springMushrooms[i].frameNumber = 1;
                    }

                    if (springMushrooms[i].animationSpeed <= 0)
                    {
                        springMushrooms[i].animationSpeed = 0;
                        springMushrooms[i].frameNumber = 0;

                    }


                }

                //player jumps on springMushroom to bounce
                if (Collision(playerRect, springMushrooms[i].springMushroomHitRect) && gravity > 0.0f && springMushrooms[i].active == true)
                {
                    gravity = -17.0f;

                    mrstickFrameNumber = 18;
                    springMushrooms[i].animationSpeed = 15;

                    //onGround = false;
                    //sounds.PlayCue("bounceFrmMush");
                    springSfx.Play(sfxVol);


                }
            }






            #endregion

            //collect coconuts
            for (int i = 0; i < 9; i++)
            {
                if (Collision(playerRect, cocoNuts[i].cocoNutRect))
                {
                    score += 3;
                    cocoNuts[i].pos.X = -500;
                    cocoNuts[i].pos.Y = -500;
                    cocoNuts[i].active = false;
                    collectCoconutSfx.Play(sfxVol);
                }
            }

            //flames hurt
            for (int i = 0; i < 20; i++)
            {
                if (Collision(playerRect, flames[i].flameHitRect) && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }
            }

            #region spikeUps

            //spikeUps hurt (do enemys)

            for (int i = 0; i < 50; i++)
            {
                if (Collision(playerRect, spikeUps[i].spikeUpHitRect) && spikeUps[i].active == true && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }





                //if the enemy is outside the play area, make it inactive
                if (spikeUps[i].pos.X < 0 || spikeUps[i].pos.X > 19900 ||
                    spikeUps[i].pos.Y < 0 || spikeUps[i].pos.Y > 5900)
                {
                    spikeUps[i].active = false;
                }



            }
            #endregion

            #region spikeDowns

            //spikeDowns hurt (do enemys)

            for (int i = 0; i < 50; i++)
            {
                if (Collision(playerRect, spikeDowns[i].spikeDownHitRect) && spikeDowns[i].active == true && playerHurt == 0)
                {
                    playerHurt = 240;
                    playerHealth--;
                    ouchSfx.Play(sfxVol);

                    //pause the game so the player can see how they died
                    if (playerHealth == 0)
                    {
                        pauseGameTemporarlyAfterDeath = 120;
                    }
                }





                //if the enemy is outside the play area, make it inactive
                if (spikeDowns[i].pos.X < 0 || spikeDowns[i].pos.X > 19900 ||
                    spikeDowns[i].pos.Y < 0 || spikeDowns[i].pos.Y > 5900)
                {
                    spikeDowns[i].active = false;
                }



            }






            #endregion


            #region control coconuts

            //for(int i=0; i<9; i++)
            //{
            //    if (cocoNuts[i].active == true)
            //    {
            //        if ((playerPos.Y > cocoNuts[i].pos.Y) &&
            //            (playerPos.Y < (cocoNuts[i].pos.Y + 250))
            //            && playerPos.X < cocoNuts[i].pos.X + 40 && playerPos.X > cocoNuts[i].pos.X - 40)
            //        {
            //            cocoNuts[i].falling = true;

            //        }

            //        if (cocoNuts[i].falling == true)
            //        {
            //            cocoNuts[i].pos.Y += 7;
            //        }

            //        if (level[(int)(cocoNuts[i].pos.Y / 40), (int)(cocoNuts[i].pos.X / 40)] > 0)
            //        {
            //            cocoNuts[i].falling = false;
            //            cocoNuts[i].frameNumber = 1;
            //        }
            //    }
            //}

            #endregion

            #region control flames

            flameAnimationSpeed++;
            if (flameAnimationSpeed == 8)
            {
                flameFrameNumber++;
                flameAnimationSpeed = 0;
            }

            if (flameFrameNumber > 1)
            {
                flameFrameNumber = 0;
            }

            for (int i = 0; i < 20; i++)
            {
                if (flames[i].active == true)
                {

                    //if the flame is droping and it hits an obstacle, make it move up
                    if (flames[i].droping == true)
                    {
                        flames[i].pos.Y += 1;

                        if (level[(int)(flames[i].pos.Y + 40) / 40, (int)(flames[i].pos.X / 40)] > 0
                            || level[(int)(flames[i].pos.Y + 40) / 40, (int)(flames[i].pos.X / 40)] == -15)
                        {
                            flames[i].droping = false;
                        }
                    }


                    //if the flame is rising and it hits an obstacle, make it move down
                    if (flames[i].droping == false)
                    {
                        flames[i].pos.Y -= 1;

                        if (level[(int)(flames[i].pos.Y / 40), (int)(flames[i].pos.X / 40)] > 0
                            || level[(int)(flames[i].pos.Y / 40), (int)(flames[i].pos.X / 40)] == -15)
                        {
                            flames[i].droping = true;
                        }
                    }


                    //if scrolling is set to lr, if the flame goes up offscreen, make it go down

                    //if scrolling is set to lr, if the flame goes down offscreen, make it go up
                }


            }

            #endregion


        }






        private void doDots()
        {
            for (int i = 0; i < 200; i++)
            {

                //if the collectable is outside the play area, make it inactive
                if (collectables[i].pos.X < 0 || collectables[i].pos.X > 19900 ||
                    collectables[i].pos.Y < 0 || collectables[i].pos.Y > 5900)
                {
                    collectables[i].active = false;
                }

                if (collectables[i].active == true)
                {







                    //if the dot is outside the play area, make it inactive
                    if (collectables[i].pos.X < 0 && collectables[i].pos.Y < 0)
                    {
                        collectables[i].active = false;
                    }








                    //if the player collides with the collectable, add to the collectable counter
                    //discard the collectable

                    if (Collision(playerRect, collectables[i].collectableRect))
                    {
                        collectables[i].active = false;
                        collectableCounter++;

                        collectDotSfx.Play(sfxVol);
                    }
                }

                if (collectables[i].active == false)
                {
                    collectables[i].pos.Y = -500;
                    collectables[i].pos.X = -500;
                }


                //enemys kick dots (discontinued)

                //get 100 dots for an extra life
                if (collectableCounter == 100)
                {
                    playerLives++;
                    collectableCounter = 0;
                    oneUpSfx.Play(sfxVol);
                }

            }


            //touch 1 ups and health ups to collect them and add to health/lives
            for (int i = 0; i < 3; i++)
            {
                if (Collision(playerRect, healthUps[i].healthUpRect))
                {
                    healthUps[i].pos.X = -100;
                    healthUps[i].pos.Y = -100;

                    healthUps[i].active = false;
                    healthUpSfx.Play(sfxVol);

                    if (playerHealth < 2)
                    {
                        playerHealth++;
                    }

                    if (playerHealth == 2)
                    {
                        score += 1000;
                    }

                }

                if (Collision(playerRect, oneUps[i].oneUpRect))
                {
                    oneUps[i].pos.X = -100;
                    oneUps[i].pos.Y = -100;
                    oneUpSfx.Play(sfxVol);

                    playerLives++;

                    oneUps[i].active = false;

                }

            }
        }

        private void updateSprites()
        {
            playerRect = new Rectangle((int)playerPos.X - (int)offSet.X, (int)playerPos.Y - (int)offSet.Y, 40, 40);



            //level exit
            levelExitRect = new Rectangle((int)levelExitPos.X - (int)offSet.X, (int)levelExitPos.Y - (int)offSet.Y, 40, 40);

            for (int i = 0; i < 200; i++)
            {
                //apply the force of gravity to each pickup
                collectables[i].pos.Y += collectables[i].yMovement;

                //update each pickups rectangle
                collectables[i].collectableRect = new Rectangle((int)collectables[i].pos.X -
                    (int)offSet.X, (int)collectables[i].pos.Y - (int)offSet.Y, 12, 12);
            }


            //control background graphics
            if (moveSpeed > 0.0f || moveSpeed < 0.0f)
            {
                bgMoving1Pos.X -= moveSpeed / 5;

            }





            //bg_near = new Rectangle((int)bg_near_pos.X - (int)offSet.X, (int)bg_near_pos.Y - (int)offSet.Y, 800, 600);
            //bg_far = new Rectangle((int)bg_far_pos.X - (int)offSet.X, (int)bg_far_pos.Y - (int)offSet.Y, 800, 600);

            bgMoving1 = new Rectangle((int)bgMoving1Pos.X, (int)bgMoving1Pos.Y, 800, 600);
            bgMoving2 = new Rectangle((int)bgMoving2Pos.X, (int)bgMoving2Pos.Y, 800, 600);


            if (bgMoving1Pos.X <= -800)
            {
                bgMoving1Pos.X = 0;
            }

            if (bgMoving1Pos.X >= 800)
            {
                bgMoving1Pos.X = 0;
            }



            if (bgMoving1Pos.X < 0)
            {
                bgMoving2Pos.X = bgMoving1Pos.X + 800;
            }

            if (bgMoving1Pos.X >= 0)
            {
                bgMoving2Pos.X = bgMoving1Pos.X - 800;
            }


            //update sheepBigs

            if (levelNumber == 6 || levelNumber == 14)
            {
                sheepBigs[0].sheepBigRect = new Rectangle((int)sheepBigs[0].pos.X -
                        (int)offSet.X, (int)sheepBigs[0].pos.Y - (int)offSet.Y, sheepBigWidth, sheepBigHeight);

                sheepBigs[0].sheepBigHitRect = new Rectangle((int)(sheepBigs[0].pos.X + 17) -
                (int)offSet.X, (int)(sheepBigs[0].pos.Y + 20) - (int)offSet.Y, sheepBigWidth - 30, sheepBigHeight - 10);
            }

            //update duckBigs

            if (levelNumber == 10 || levelNumber == 14)
            {
                duckBigs[0].duckBigRect = new Rectangle((int)duckBigs[0].pos.X -
                        (int)offSet.X, (int)duckBigs[0].pos.Y - (int)offSet.Y, duckBigWidth, duckBigHeight);

                duckBigs[0].duckBigHitRect = new Rectangle((int)(duckBigs[0].pos.X + 17) -
                (int)offSet.X, (int)(duckBigs[0].pos.Y + 20) - (int)offSet.Y, duckBigWidth - 30, duckBigHeight - 10);
            }

            //update frogBoss

            if (levelNumber == 19)
            {
                frogBossRect = new Rectangle((int)frogBossPos.X -
                        (int)offSet.X, (int)frogBossPos.Y - (int)offSet.Y, 302, 213);

                frogBossHitRect = new Rectangle((int)(frogBossPos.X + 56) -
                (int)offSet.X, (int)(frogBossPos.Y + 20) - (int)offSet.Y, 190, 190);
            }

            //update frogFlames
            for (int i = 0; i < 20; i++)
            {
                frogFlameRect = new Rectangle((int)frogFlamePos.X -
                    (int)offSet.X, (int)frogFlamePos.Y - (int)offSet.Y, 80, 80);

                frogFlameHitRect = new Rectangle((int)(frogFlamePos.X + 5) -
                    (int)offSet.X, (int)(frogFlamePos.Y + 5) - (int)offSet.Y, 70, 70);
            }


            // coconuts
            for (int i = 0; i < 9; i++)
            {
                cocoNuts[i].cocoNutRect = new Rectangle((int)cocoNuts[i].pos.X -
                    (int)offSet.X, (int)cocoNuts[i].pos.Y - (int)offSet.Y, 32, 32);
            }

            // flames
            for (int i = 0; i < 20; i++)
            {
                flames[i].flameRect = new Rectangle((int)flames[i].pos.X -
                    (int)offSet.X, (int)flames[i].pos.Y - (int)offSet.Y, 40, 40);

                flames[i].flameHitRect = new Rectangle((int)(flames[i].pos.X + 3) -
                    (int)offSet.X, (int)(flames[i].pos.Y + 3) - (int)offSet.Y, 35, 35);
            }



            // ducks (update ducks)
            for (int i = 0; i < 10; i++)
            {
                ducks[i].duckRect = new Rectangle((int)ducks[i].pos.X -
                    (int)offSet.X, (int)ducks[i].pos.Y - (int)offSet.Y, duckWidth, duckHeight);

                ducks[i].duckHitRect = new Rectangle((int)(ducks[i].pos.X + 17) -
                (int)offSet.X, (int)(ducks[i].pos.Y + 42) - (int)offSet.Y, duckWidth - 43, duckHeight - 70);
            }

            // hedgehogs (update hedgehogs)
            for (int i = 0; i < 10; i++)
            {
                hedgehogs[i].hedgehogRect = new Rectangle((int)hedgehogs[i].pos.X -
                    (int)offSet.X, (int)hedgehogs[i].pos.Y - (int)offSet.Y, hedgehogWidth, hedgehogHeight);

                hedgehogs[i].hedgehogHitRect = new Rectangle((int)(hedgehogs[i].pos.X + 3) -
                (int)offSet.X, (int)(hedgehogs[i].pos.Y + 3) - (int)offSet.Y, hedgehogWidth - 3, hedgehogHeight - 3);
            }

            // ladybirds (update ladybirds)
            for (int i = 0; i < 10; i++)
            {
                ladybirds[i].ladybirdRect = new Rectangle((int)ladybirds[i].pos.X -
                    (int)offSet.X, (int)ladybirds[i].pos.Y - (int)offSet.Y, ladybirdWidth, ladybirdHeight);

                ladybirds[i].jumpOnLadybirdRect = new Rectangle((int)ladybirds[i].pos.X -
                    (int)offSet.X, (int)(ladybirds[i].pos.Y - 20) - (int)offSet.Y, ladybirdWidth, 45);

                ladybirds[i].ladybirdHitRect = new Rectangle((int)(ladybirds[i].pos.X + 3) -
                (int)offSet.X, (int)(ladybirds[i].pos.Y) - (int)offSet.Y, ladybirdWidth - 6, ladybirdHeight);
            }

            // sheeps (update sheeps)
            for (int i = 0; i < 10; i++)
            {
                sheeps[i].sheepRect = new Rectangle((int)sheeps[i].pos.X -
                    (int)offSet.X, (int)sheeps[i].pos.Y - (int)offSet.Y, sheepWidth, sheepHeight);

                sheeps[i].sheepHitRect = new Rectangle((int)(sheeps[i].pos.X + 3) -
                (int)offSet.X, (int)(sheeps[i].pos.Y + 3) - (int)offSet.Y, sheepWidth - 3, sheepHeight - 3);
            }

            // snails (update snails)
            for (int i = 0; i < 10; i++)
            {
                snails[i].snailRect = new Rectangle((int)snails[i].pos.X -
                    (int)offSet.X, (int)snails[i].pos.Y - (int)offSet.Y, snailWidth, snailHeight);

                snails[i].snailHitRect = new Rectangle((int)(snails[i].pos.X + 5) -
                (int)offSet.X, (int)(snails[i].pos.Y + 5) - (int)offSet.Y, snailWidth - 5, snailHeight - 5);
            }

            // turtles (update turtles)
            for (int i = 0; i < 10; i++)
            {
                turtles[i].turtleRect = new Rectangle((int)turtles[i].pos.X -
                    (int)offSet.X, (int)turtles[i].pos.Y - (int)offSet.Y, turtleWidth, turtleHeight);

                turtles[i].turtleHitRect = new Rectangle((int)(turtles[i].pos.X + 3) -
                (int)offSet.X, (int)(turtles[i].pos.Y + 3) - (int)offSet.Y, turtleWidth - 3, turtleHeight - 3);

                turtles[i].jumpOnTurtleRect = new Rectangle((int)turtles[i].pos.X -
                    (int)offSet.X, (int)(turtles[i].pos.Y - 20) - (int)offSet.Y, turtleWidth, 45);
            }

            // bees (update bees)
            for (int i = 0; i < 10; i++)
            {
                bees[i].beeRect = new Rectangle((int)bees[i].pos.X -
                    (int)offSet.X, (int)bees[i].pos.Y - (int)offSet.Y, beeWidth, beeHeight);

                bees[i].beeHitRect = new Rectangle((int)(bees[i].pos.X + 10) -
                (int)offSet.X, (int)(bees[i].pos.Y + 26) - (int)offSet.Y, beeWidth - 19, beeHeight - 42);
            }

            // birds (update birds)
            for (int i = 0; i < 10; i++)
            {
                birds[i].birdRect = new Rectangle((int)birds[i].pos.X -
                    (int)offSet.X, (int)birds[i].pos.Y - (int)offSet.Y, birdWidth, birdHeight);

                birds[i].birdHitRect = new Rectangle((int)(birds[i].pos.X + 2) -
                (int)offSet.X, (int)(birds[i].pos.Y + 2) - (int)offSet.Y, birdWidth - 12, birdHeight - 17);
            }

            // fishs (update fishs)
            for (int i = 0; i < 10; i++)
            {
                fishs[i].fishRect = new Rectangle((int)fishs[i].pos.X -
                    (int)offSet.X, (int)fishs[i].pos.Y - (int)offSet.Y, fishWidth, fishHeight);

                fishs[i].fishHitRect = new Rectangle((int)(fishs[i].pos.X + 3) -
                (int)offSet.X, (int)(fishs[i].pos.Y + 3) - (int)offSet.Y, fishWidth - 3, fishHeight - 3);
            }



            //one ups and health ups
            for (int i = 0; i < 3; i++)
            {
                oneUps[i].oneUpRect = new Rectangle((int)oneUps[i].pos.X - (int)offSet.X,
                    (int)oneUps[i].pos.Y - (int)offSet.Y, 40, 40);

                healthUps[i].healthUpRect = new Rectangle((int)healthUps[i].pos.X - (int)offSet.X,
                    (int)healthUps[i].pos.Y - (int)offSet.Y, 40, 40);
            }

            // spikeUps (update spikeUps)
            for (int i = 0; i < 50; i++)
            {
                spikeUps[i].spikeUpRect = new Rectangle((int)spikeUps[i].pos.X -
                    (int)offSet.X, (int)spikeUps[i].pos.Y - (int)offSet.Y, 12, 12);

                spikeUps[i].spikeUpHitRect = new Rectangle((int)(spikeUps[i].pos.X + 2) -
                (int)offSet.X, (int)(spikeUps[i].pos.Y + 2) - (int)offSet.Y, 10, 10);
            }


            // spikeDowns (update spikeDowns)
            for (int i = 0; i < 50; i++)
            {
                spikeDowns[i].spikeDownRect = new Rectangle((int)spikeDowns[i].pos.X -
                    (int)offSet.X, (int)spikeDowns[i].pos.Y - (int)offSet.Y, 12, 12);

                spikeDowns[i].spikeDownHitRect = new Rectangle((int)(spikeDowns[i].pos.X + 2) -
                (int)offSet.X, (int)(spikeDowns[i].pos.Y + 2) - (int)offSet.Y, 10, 10);
            }

            // springMushrooms (update springMushrooms)
            for (int i = 0; i < 10; i++)
            {
                springMushrooms[i].springMushroomRect = new Rectangle((int)springMushrooms[i].pos.X -
                    (int)offSet.X, (int)springMushrooms[i].pos.Y - (int)offSet.Y, springMushroomWidth, springMushroomHeight);

                springMushrooms[i].springMushroomHitRect = new Rectangle((int)(springMushrooms[i].pos.X + 5) -
                (int)offSet.X, (int)(springMushrooms[i].pos.Y + 32) - (int)offSet.Y, springMushroomWidth - 10, springMushroomHeight - 55);
            }


            //if the player touches an exit, move the game onto the next level
            if (Collision(playerRect, levelExitRect) && levelNumber < 90)
            {

                levelExitPos.X = -500;
                levelExitPos.Y = -500;

                //reset the playerSprite movement variables
                gravity = 0.0f;
                moveSpeed = 0.0f;

                previousLevelNumber = levelNumber += 1;

                levelNumber = 99;

                loadLevel("level" + levelNumber);


                //use the start of level method to set the game sprites initial positions
                // startOfLevel();
            }







        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            //intro
            if (levelNumber == 95 || levelNumber == 96)
            {

                spriteBatch.Draw(bg[bgStaticToShow], bgStatic, Color.White);
                spriteBatch.Draw(bg[bgMovingToShow], bgMoving1, Color.White);
                spriteBatch.Draw(bg[bgMovingToShow], bgMoving2, Color.White);


            }


            //title screen
            if (levelNumber == 97)
            {
                spriteBatch.Draw(titleScreen, new Rectangle(0, 0, 800, 600), Color.White);

                spriteBatch.Draw(bg[8], bgMoving1, new Color(255, 255, 255, 200));
                spriteBatch.Draw(bg[8], bgMoving2, new Color(255, 255, 255, 200));

                
                bgMoving1Pos.X--;

                if (bgMoving1Pos.X <= -800)
                {
                    bgMoving1Pos.X = 800;
                }
                

            }

            //level start screen
            if (levelNumber == 99)
            {


                //spriteBatch.DrawString(courier, levelNames[previousLevelNumber], new Vector2(380, 300), Color.White);

                spriteBatch.DrawString(courier, levelNames[previousLevelNumber],
                    new Vector2(400 - ((levelNames[previousLevelNumber].Length * 12) / 2), 294), Color.White);

                playerPos.Y = 500;
                playerPos.X = 500;


            }

            //game over screen
            if (levelNumber == 98)
            {
                playerPos.Y = 500;
                playerPos.X = 500;

                spriteBatch.DrawString(courier, "Game Over",
                    new Vector2(400 - ((levelNames[previousLevelNumber].Length * 12) / 2), 294), Color.Red);
            }

            //draw backgrounds before anything else is drawn so they are not in front of other sprites


            if (levelNumber < 90)
            {
                //level backgrounds

                //if (levelNumber == 2)
                //{
                //    spriteBatch.Draw(bg3, bgStatic, Color.White);
                //    spriteBatch.Draw(bg6, bgMoving1, Color.White);


                //    spriteBatch.Draw(bg6, bgMoving2, Color.White);
                //}

                spriteBatch.Draw(bg[bgStaticToShow], bgStatic, Color.White);
                spriteBatch.Draw(bg[bgMovingToShow], bgMoving1, Color.White);
                spriteBatch.Draw(bg[bgMovingToShow], bgMoving2, Color.White);
            }

            //draw the level

            for (int y = 0; y < tilesDown; y++)
            {
                for (int x = 0; x < tilesAcross; x++)
                {
                    if (level[y, x] < 0)
                    {
                        //only draw visible tiles
                        if (x * 40 > playerPos.X - 800 && x * 40 < playerPos.X + 800 && y * 40 < playerPos.Y + 1200 && y * 40 > playerPos.Y - 1200)
                        {

                            spriteBatch.Draw(passableTiles[level[y, x] * -1], new Rectangle
                                    (x * 40 - (int)offSet.X, y * 40 - (int)offSet.Y, tileWidth, tileHeight), Color.White);


                        }
                    }

                    if (level[y, x] > 0)
                    {
                        //only draw visible tiles
                        if (x * 40 > playerPos.X - 800 && x * 40 < playerPos.X + 800 && y * 40 < playerPos.Y + 1200 && y * 40 > playerPos.Y - 1200)
                        {
                            spriteBatch.Draw(solidTiles[level[y, x]], new Rectangle
                                (x * 40 - (int)offSet.X, y * 40 - (int)offSet.Y, tileWidth, tileHeight), Color.White);
                        }
                    }
                }
            }



            //only draw the player if its a gameplay level
            if (levelNumber < 90)
            {
                //draw player
                if (playerHurt <= 0)
                {
                    spriteBatch.Draw(mrstickSprite, playerRect, mrstickFrames[mrstickFrameNumber + showLeftOrRight], Color.White);
                }

                if (playerHurt > 0)
                {
                    spriteBatch.Draw(mrstickSprite, playerRect, mrstickFrames[mrstickFrameNumber + showLeftOrRight], Color.Gray);

                }

                //if the player has died, make mrstick black to show player how it happened
                if (pauseGameTemporarlyAfterDeath > 0)
                {
                    spriteBatch.Draw(mrstickSprite, playerRect, mrstickFrames[mrstickFrameNumber + showLeftOrRight], Color.Black);
                }
            }


            //draw spikes
            for (int i = 0; i < 50; i++)
            {
                if (spikeUps[i].active == true)
                {
                    spriteBatch.Draw(spikeUpSprite, spikeUps[i].pos - offSet, spikeUpRect, Color.White);

                }

                if (spikeDowns[i].active == true)
                {
                    spriteBatch.Draw(spikeDownSprite, spikeDowns[i].pos - offSet, spikeDownRect, Color.White);

                }
            }

            for (int i = 0; i < 10; i++)
            {
                if (springMushrooms[i].active == true)
                {
                    spriteBatch.Draw(springMushroomSprite, springMushrooms[i].pos - offSet, springMushroomFrames[springMushrooms[i].frameNumber], Color.White);

                }
            }


            #region draw enemys

            //draw sheepbigs
            if (levelNumber == 6 || levelNumber == 14)
            {
                if (sheepBigs[0].active == true)
                {
                    if (sheepBigs[0].goingRight == false)
                    {
                        if (sheepBigs[0].hurt == 0)
                        {
                            spriteBatch.Draw(sheepBigSprite, sheepBigs[0].pos - offSet, sheepBigFrames[sheepBigFrameNumber + 2], Color.White);
                        }

                        if (sheepBigs[0].hurt > 0)
                        {
                            spriteBatch.Draw(sheepBigSprite, sheepBigs[0].pos - offSet, sheepBigFrames[sheepBigFrameNumber + 2], Color.Blue);
                        }
                    }
                    else
                    {
                        if (sheepBigs[0].hurt == 0)
                        {
                            spriteBatch.Draw(sheepBigSprite, sheepBigs[0].pos - offSet, sheepBigFrames[sheepBigFrameNumber], Color.White);
                        }

                        if (sheepBigs[0].hurt > 0)
                        {
                            spriteBatch.Draw(sheepBigSprite, sheepBigs[0].pos - offSet, sheepBigFrames[sheepBigFrameNumber], Color.Blue);
                        }
                    }



                }
            }

            //draw duckbigs
            if (levelNumber == 10 || levelNumber == 14)
            {
                if (duckBigs[0].active == true)
                {
                    if (duckBigs[0].goingRight == false)
                    {
                        if (duckBigs[0].hurt == 0)
                        {
                            spriteBatch.Draw(duckBigSprite, duckBigs[0].pos - offSet, duckBigFrames[duckBigFrameNumber + 4], Color.White);
                        }

                        if (duckBigs[0].hurt > 0)
                        {
                            spriteBatch.Draw(duckBigSprite, duckBigs[0].pos - offSet, duckBigFrames[duckBigFrameNumber + 4], Color.Blue);
                        }
                    }
                    else
                    {
                        if (duckBigs[0].hurt == 0)
                        {
                            spriteBatch.Draw(duckBigSprite, duckBigs[0].pos - offSet, duckBigFrames[duckBigFrameNumber], Color.White);
                        }

                        if (duckBigs[0].hurt > 0)
                        {
                            spriteBatch.Draw(duckBigSprite, duckBigs[0].pos - offSet, duckBigFrames[duckBigFrameNumber], Color.Blue);
                        }
                    }



                }
            }

            for (int i = 0; i < 10; i++)
            {

                //draw ducks
                if (ducks[i].active == true)
                {
                    if (ducks[i].goingRight == false)
                    {
                        spriteBatch.Draw(duckSprite, ducks[i].pos - offSet, duckFrames[duckFrameNumber + 4], Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(duckSprite, ducks[i].pos - offSet, duckFrames[duckFrameNumber], Color.White);
                    }
                }

                //draw bees
                if (bees[i].active == true)
                {
                    if (bees[i].goingRight == false)
                    {
                        spriteBatch.Draw(beeSprite, bees[i].pos - offSet, beeFrames[beeFrameNumber + 2], Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(beeSprite, bees[i].pos - offSet, beeFrames[beeFrameNumber], Color.White);
                    }
                }

                //draw birds
                if (birds[i].active == true)
                {
                    if (birds[i].goingRight == false)
                    {
                        spriteBatch.Draw(birdSprite, birds[i].pos - offSet, birdFrames[birdFrameNumber + 4], Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(birdSprite, birds[i].pos - offSet, birdFrames[birdFrameNumber], Color.White);
                    }
                }


                //draw fish
                if (fishs[i].active == true)
                {
                    if (fishs[i].goingUp == false)
                    {
                        spriteBatch.Draw(fishSprite, fishs[i].pos - offSet, fishFrames[fishFrameNumber + 2], Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(fishSprite, fishs[i].pos - offSet, fishFrames[fishFrameNumber], Color.White);
                    }
                }

                //draw hedgehogs
                if (hedgehogs[i].active == true)
                {
                    if (hedgehogs[i].goingRight == false)
                    {
                        spriteBatch.Draw(hedgehogSprite, hedgehogs[i].pos - offSet, hedgehogFrames[hedgehogFrameNumber + 2], Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(hedgehogSprite, hedgehogs[i].pos - offSet, hedgehogFrames[hedgehogFrameNumber], Color.White);
                    }
                }

                //draw ladybirds
                if (ladybirds[i].active == true)
                {
                    if (ladybirds[i].goingRight == false)
                    {
                        spriteBatch.Draw(ladybirdSprite, ladybirds[i].pos - offSet, ladybirdFrames[ladybirdFrameNumber + 2], Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(ladybirdSprite, ladybirds[i].pos - offSet, ladybirdFrames[ladybirdFrameNumber], Color.White);
                    }
                }

                //draw sheep
                if (sheeps[i].active == true)
                {
                    if (sheeps[i].goingRight == false)
                    {
                        spriteBatch.Draw(sheepSprite, sheeps[i].pos - offSet, sheepFrames[sheepFrameNumber + 2], Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(sheepSprite, sheeps[i].pos - offSet, sheepFrames[sheepFrameNumber], Color.White);
                    }
                }
                //draw snails
                if (snails[i].active == true)
                {
                    if (snails[i].goingRight == false)
                    {
                        spriteBatch.Draw(snailSprite, snails[i].pos - offSet, snailFrames[snailFrameNumber + 5], Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(snailSprite, snails[i].pos - offSet, snailFrames[snailFrameNumber], Color.White);
                    }
                }

                //draw turtles
                if (turtles[i].active == true)
                {
                    if (turtles[i].goingRight == false)
                    {
                        spriteBatch.Draw(turtleSprite, turtles[i].pos - offSet, turtleFrames[turtleFrameNumber + 3], Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(turtleSprite, turtles[i].pos - offSet, turtleFrames[turtleFrameNumber], Color.White);
                    }
                }
            }





            #endregion

            //draw coconuts
            for (int i = 0; i < 9; i++)
            {
                if (cocoNuts[i].active == true)
                {
                    spriteBatch.Draw(cocoNutSprite, cocoNuts[i].pos - offSet, cocoNutFrames[cocoNuts[i].frameNumber], Color.White);
                }
            }

            //draw flames
            for (int i = 0; i < 20; i++)
            {
                if (flames[i].active == true)
                {
                    spriteBatch.Draw(flameSprite, flames[i].pos - offSet, flameFrames[flameFrameNumber], Color.White);
                }
            }

            #region draw collectables, 1ups, healthups, playerstart, level exit

            for (int i = 0; i < 200; i++)
            {
                if (collectables[i].active == true)
                {
                    spriteBatch.Draw(collectableSprite, collectables[i].pos - offSet, Color.White);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (oneUps[i].active == true)
                {
                    spriteBatch.Draw(oneUpSprite, oneUps[i].pos - offSet, Color.White);
                }

                if (healthUps[i].active == true)
                {
                    spriteBatch.Draw(healthUpSprite, healthUps[i].pos - offSet, Color.White);
                }
            }

            //spriteBatch.Draw(playerStart, playerStartPos - offSet, Color.White);
            spriteBatch.Draw(levelExitSprite, levelExitPos - offSet, Color.White);


            //hud
            //draw the health meter on the hud
            if (levelNumber < 90 && playerHealth >= 0)   //if playerActive==true
            {
                spriteBatch.Draw(healthMeter, new Rectangle(755, 10, 40, 40), healthMeterRect[playerHealth], Color.White);
            }

            if (levelNumber < 90)
            {
                spriteBatch.DrawString(textSmall, "Lives", new Vector2(5, 3), Color.Red);
                spriteBatch.DrawString(textSmall, "Score", new Vector2(300, 3), Color.Orange);
                spriteBatch.DrawString(textSmall, "Dots", new Vector2(500, 3), Color.Red);
                //spriteBatch.DrawString(textSmall, "Health", new Vector2(735, 3), Color.Orange);



                spriteBatch.DrawString(text1, playerLives.ToString(), new Vector2(40, 20), Color.Orange);
                spriteBatch.DrawString(text1, score.ToString(), new Vector2(310, 20), Color.Red);
                spriteBatch.DrawString(text1, collectableCounter.ToString(), new Vector2(520, 20), Color.Orange);
            }

            #endregion




            //draw frog boss
            if (levelNumber == 19)
            {

                if (frogBossHurt == 0)
                {
                    spriteBatch.Draw(frogBoss, frogBossPos - offSet, frogBossFrames[frogBossFrameNumber], Color.White);
                }

                if (frogBossHurt > 0)
                {
                    spriteBatch.Draw(frogBoss, frogBossPos - offSet, frogBossFrames[frogBossFrameNumber], Color.Blue);
                }


                //draw frogFlames
                if (frogFlameActive == true)
                {
                    spriteBatch.Draw(frogFlameSprite, frogFlamePos - offSet, frogFlameFrames[frogFlameFrameNumber], Color.White);
                }


            }

            
            //intro drawing code
            if (levelNumber == 95)
            {
                //spriteBatch.Draw(blackBorder, new Rectangle(0, 0, 800, 70), Color.White);
                //spriteBatch.Draw(blackBorder, new Rectangle(0, 530, 800, 70), Color.White);
                spriteBatch.Draw(kickRed, new Rectangle((int)kickRedPos.X - (int)offSet.X,
                    (int)kickRedPos.Y - (int)offSet.Y, 40, 40), kickRedRect[kickRedFrameNumber], Color.White);

                spriteBatch.Draw(kickBlue, new Rectangle((int)kickBluePos.X - (int)offSet.X,
                    (int)kickBluePos.Y - (int)offSet.Y, 40, 40), kickBlueRect[kickBlueFrameNumber], Color.White);

                spriteBatch.Draw(introBall, new Rectangle((int)introBallPos.X - (int)offSet.X,
                    (int)introBallPos.Y - (int)offSet.Y, 32, 32), introBallRect[introBallFrameNumber], Color.White);

                if (introTimer > 1000)
                {
                    spriteBatch.Draw(frogBoss, frogBossPos - offSet, frogBossFrames[frogBossFrameNumber], Color.White);
                }

                //if (introTimer > 2500 && introTimer < 2505)
                //{
                //    spriteBatch.Draw(introLightning, new Vector2(playerPos.X - offSet.X, playerPos.Y - offSet.Y), Color.White);
                //}
                if (introTimer > 2500 && introTimer < 2505)
                {
                    spriteBatch.Draw(introLightning, new Vector2(frogBossPos.X - 150 -
                        offSet.X, frogBossPos.Y + 300 - offSet.Y), Color.White);
                }

                if (introTimer > 2510 && introTimer < 2515)
                {
                    spriteBatch.Draw(introLightning, new Vector2(frogBossPos.X - 150 -
                        offSet.X, frogBossPos.Y + 300 - offSet.Y), Color.White);
                }

                if (introTimer > 2520 && introTimer < 2525)
                {
                    spriteBatch.Draw(introLightning, new Vector2(frogBossPos.X - 150 -
                        offSet.X, frogBossPos.Y + 300 - offSet.Y), Color.White);
                }

                if (introTimer > 2530 && introTimer < 2535)
                {
                    spriteBatch.Draw(introLightning, new Vector2(frogBossPos.X - 150 -
                        offSet.X, frogBossPos.Y + 300 - offSet.Y), Color.White);
                }

                if (introTimer > 2540 && introTimer < 2545)
                {
                    spriteBatch.Draw(introLightning, new Vector2(frogBossPos.X - 150 -
                        offSet.X, frogBossPos.Y + 300 - offSet.Y), Color.White);
                }

                if (introTimer > 2550 && introTimer < 2555)
                {
                    spriteBatch.Draw(introLightning, new Vector2(frogBossPos.X - 150 -
                        offSet.X, frogBossPos.Y + 300 - offSet.Y), Color.White);
                }

                //play lightning sound
                if (introTimer == 2510 || introTimer == 2520 || introTimer == 2530 || introTimer == 2540 ||
                    introTimer == 2550)
                {
                    lightningSfx.Play(sfxVol);
                }

                if (introTimer > 2560 && introTimer < 2580)
                {
                    kickBluePos.Y -= 3;
                    kickRedPos.Y -= 3;
                }

                if (introTimer > 2580 && introTimer < 2600)
                {
                    kickBluePos.Y -= 2;
                    kickRedPos.Y -= 2;
                }

                if (introTimer > 2600 && introTimer < 2620)
                {
                    kickBluePos.Y -= 1;
                    kickRedPos.Y -= 1;
                }

                if (introTimer > 2580 && introTimer < 3100)
                {
                    frogBossPos.Y -= 10;
                    frogBossFrameNumber = 0;

                    

                    playerPos.Y += 2;
                }

                if (introTimer > 2620 && introTimer < 2640)
                {
                    kickBluePos.Y += 1;
                    kickRedPos.Y += 1;
                }

                if (introTimer > 2640 && introTimer < 2660)
                {
                    kickBluePos.Y += 2;
                    kickRedPos.Y += 2;
                }

                if (introTimer > 2660 && introTimer < 2680)
                {
                    kickBluePos.Y += 3;
                    kickRedPos.Y += 3;
                }

                if (introTimer > 2680 && introTimer < 2700)
                {
                    kickBluePos.Y += 4;
                    kickRedPos.Y += 4;
                }

                if (introTimer > 2700 && introTimer < 2720)
                {
                    kickBluePos.Y += 5;
                    kickRedPos.Y += 4;
                }

                if (introTimer > 2720 && introTimer < 3100)
                {
                    kickBluePos.Y += 6;
                    kickRedPos.Y += 5;
                }

                //display intro text
                if (introTimer > 100 && introTimer < 300)
                {
                    spriteBatch.Draw(introText0, new Vector2(400 - introText0.Width / 2, 500 - introText0.Height), Color.White);
                }
                if (introTimer > 500 && introTimer < 700)
                {
                    spriteBatch.Draw(introText1, new Vector2(400 - introText1.Width / 2, 500 - introText1.Height), Color.White);
                }
                if (introTimer > 900 && introTimer < 1100)
                {
                    spriteBatch.Draw(introText2, new Vector2(400 - introText2.Width / 2, 500 - introText2.Height), Color.White);
                }
                if (introTimer > 1300 && introTimer < 1500)
                {
                    spriteBatch.Draw(introText3, new Vector2(400 - introText3.Width / 2, 500 - introText3.Height), Color.White);
                }
                if (introTimer > 1700 && introTimer < 1900)
                {
                    spriteBatch.Draw(introText4, new Vector2(400 - introText4.Width / 2, 500 - introText4.Height), Color.White);
                }
                if (introTimer > 2100 && introTimer < 2300)
                {
                    spriteBatch.Draw(introText5, new Vector2(400 - introText5.Width / 2, 500 - introText5.Height), Color.White);
                }
                if (introTimer > 3000 && introTimer < 3200)
                {
                    spriteBatch.Draw(introText6, new Vector2(400 - introText6.Width / 2, 500 - introText6.Height), Color.White);
                }
                if (introTimer > 3400 && introTimer < 3600)
                {
                    spriteBatch.Draw(introText7, new Vector2(400 - introText7.Width / 2, 500 - introText7.Height), Color.White);
                }


            }

            if (levelNumber == 96)
            {
                spriteBatch.Draw(kickRed, new Rectangle((int)kickRedPos.X - (int)offSet.X,
                        (int)kickRedPos.Y - (int)offSet.Y, 40, 40), kickRedRect[kickRedFrameNumber], Color.White);

                spriteBatch.Draw(kickBlue, new Rectangle((int)kickBluePos.X - (int)offSet.X,
                    (int)kickBluePos.Y - (int)offSet.Y, 40, 40), kickBlueRect[kickBlueFrameNumber], Color.White);

                spriteBatch.Draw(introBall, new Rectangle((int)introBallPos.X - (int)offSet.X,
                    (int)introBallPos.Y - (int)offSet.Y, 32, 32), introBallRect[introBallFrameNumber], Color.DeepSkyBlue);

                if (endingTimer > 580 && endingTimer < 780)
                {
                    spriteBatch.Draw(introText8, new Vector2(400 - introText8.Width / 2, 500 - introText8.Height), Color.White);
                }
                if (endingTimer > 980 && endingTimer < 1180)
                {
                    spriteBatch.Draw(introText9, new Vector2(400 - introText9.Width / 2, 500 - introText9.Height), Color.White);
                }

                if (endingTimer > 1380 && endingTimer < 1580)
                {
                    spriteBatch.Draw(introText10, new Vector2(400 - introText10.Width / 2, 500 - introText10.Height), Color.White);
                }
            }

            drawDebugInformation();


            
            
            #region draw menu's
            
            //main menu
            if (menuShowing == "main" && levelNumber==97)
            {
                spriteBatch.Draw(menuMain, new Vector2(325, 475), Color.White);
                spriteBatch.Draw(menuPointer, menuPointerPos, Color.White);
            }

            

            //options menu
            if (menuShowing == "options" && levelNumber==97)
            {
                spriteBatch.Draw(menuOptions, new Vector2(325, 475), Color.White);
                spriteBatch.Draw(menuPointer, menuPointerPos, Color.White);

                spriteBatch.Draw(menuSlider, menuSlider1Pos, Color.White);
                spriteBatch.Draw(menuSlider2, menuSlider2Pos, Color.White);

            }

            //exit game menu
            if (menuShowing == "exitGame" && levelNumber==97)
            {
                spriteBatch.Draw(menuExit, new Vector2(325, 225), Color.White);
                spriteBatch.Draw(menuPointer, menuPointerPos, Color.White);
            }

            //paused menu
            if (menuShowing == "paused" && levelNumber < 90 && paused == true)
            {
                spriteBatch.Draw(menuPaused, new Vector2(349, 225), Color.White);
                spriteBatch.Draw(menuPointer, menuPointerPos, Color.White);
            }

            


            #endregion

            spriteBatch.End();

            base.Draw(gameTime);
        }



        private void scrolling()
        {


            if (scrollingType == "standard")
            {
                scrollerPos = playerPos;

                if (levelNumber == 15)
                {
                    //if the player falls off the level
                    //kill player
                    if (playerPos.X > 6280 && playerPos.Y > 4080)
                    {
                        gravity = 0;
                        playerHealth = 0;
                        pauseGameTemporarlyAfterDeath = 120;
                        scrollingType = "";

                    }
                }

                if (levelNumber == 17)
                {
                    //if the player falls off the level
                    //kill player
                    if (playerPos.Y >= 5720)
                    {
                        gravity = 0;
                        playerHealth = 0;
                        pauseGameTemporarlyAfterDeath = 120;
                        scrollingType = "";

                    }
                }


            }

            if (scrollingType == "intro")
            {
                scrollerPos = playerPos;

                if (playerPos.X < 6800)
                {
                    playerPos.X += 2;
                }
            }


            if (scrollingType == "ending")
            {
                scrollerPos = playerPos;

                if (endingTimer < 560)
                {
                    playerPos.Y += 2;
                }



                if (endingTimer == 1680)
                {
                    levelNumber = 97; //go to titleScreen
                    //previousLevelNumber = 1;
                    loadLevel("level" + levelNumber);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && keyPressed == false)
                {
                    keyPressed = true;
                    levelNumber = 97; //go to titleScreen
                    //previousLevelNumber = 1;
                    loadLevel("level" + levelNumber);
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Enter))
                {
                    keyPressed = false;
                }
            }




            if (scrollingType == "titleScreen")
            {
                scrollerPos.X = 0;
                scrollerPos.Y = 0;
            }



            if (scrollingType == "lr")
            {
                scrollerPos.X = playerPos.X;
                scrollerPos.Y = 860;


                //if the player falls off the level
                //kill player
                if (playerPos.Y > 1260)
                {
                    gravity = 0;
                    playerHealth = 0;
                    pauseGameTemporarlyAfterDeath = 120;
                    scrollingType = "";

                }
            }

            if (scrollingType == "lr2")
            {
                scrollerPos.X = playerPos.X;
                scrollerPos.Y = 5100;


                //if the player falls off the level
                //kill player
                if (playerPos.Y > 5500)
                {
                    gravity = 0;
                    playerHealth = 0;
                    pauseGameTemporarlyAfterDeath = 120;
                    scrollingType = "";

                }
            }

            if (scrollingType == "ddd")
            {
                scrollerPos.X = 1280;
                scrollerPos.Y = playerPos.Y;
            }

            if (scrollingType == "shake")
            {
                shakeTimer--;

                if (shakeTimer == 9)
                {
                    scrollerPos.Y += 40;
                }

                if (shakeTimer == 8)
                {
                    scrollerPos.Y -= 40;
                }

                if (shakeTimer == 7)
                {
                    scrollerPos.Y -= 40;
                }

                if (shakeTimer == 6)
                {
                    scrollerPos.Y += 40;
                }

                if (shakeTimer == 5)
                {
                    scrollerPos.Y += 40;
                }

                if (shakeTimer == 4)
                {
                    scrollerPos.Y -= 40;
                }

                if (shakeTimer == 3)
                {
                    scrollerPos.Y -= 40;
                }

                if (shakeTimer == 2)
                {
                    scrollerPos.Y += 40;
                }

                if (shakeTimer == 1)
                {
                    scrollerPos.Y += 40;
                }


                if (shakeTimer <= 0)
                {
                    scrollingType = "standard";
                }
            }
        }

        void doPlayer()
        {


            doLeftRightMovement();

            doJumpFall();

            doPlayerAnimation();

            // doSlopes();
        }

        private void doPlayerAnimation()
        {

            if (playerFacingRight == false)
            {
                showLeftOrRight = 24;
            }

            if (playerFacingRight == true)
            {
                showLeftOrRight = 0;
            }

            if ((moveSpeed == 0.0f) && (onGround == true))
            {
                mrstickFrameNumber = 0;
            }


            //walking left and right<<<
            if ((moveSpeed != 0.0f) && (onGround == true))
            {
                //if (mrstickFrameNumber == 47)
                //{

                //}

                //if (mrstickFrameNumber < 47)
                //{
                mrstickAnimationSpeed++;
                if (mrstickAnimationSpeed == 2)
                {
                    mrstickAnimationSpeed = 0;
                    mrstickFrameNumber++;
                }

                //}

                if (mrstickFrameNumber >= 17)
                {
                    mrstickFrameNumber = 6;
                }
            }


            //jumping up
            if ((onGround == false) && (gravity < 0.0f))
            {
                if (mrstickFrameNumber < 22)
                {
                    mrstickAnimationSpeed++;
                    if (mrstickAnimationSpeed == 5)
                    {
                        mrstickAnimationSpeed = 0;
                        mrstickFrameNumber++;
                    }
                }
                else
                {
                    mrstickFrameNumber = 22;
                }
            }

            //falling down
            if ((onGround == false) && (gravity >= 0.0f))
            {
                mrstickFrameNumber = 23;
            }

            if (mrstickAnimationSpeed > 5)
            {
                mrstickAnimationSpeed = 0;
            }

        }

        bool Collision(Rectangle r1, Rectangle r2)
        {
            return !(r2.Left > r1.Right || r2.Right < r1.Left || r2.Top > r1.Bottom || r2.Bottom < r1.Top);
        }

        private void doLeftRightMovement()
        {
            //** Left>>>>>

            //press left key to subtract .25 from moveSpeed, make the character face left
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                moveSpeed -= 0.25f;
                playerFacingRight = false;
            }

            //moving left
            if (moveSpeed < 0.0f)
            {
                movingRight = false;

                //if the player will NOT run into an obstacle background on the next frame, move the player
                if (level[((int)((playerPos.Y + playerHeight) - 1) / 40), (int)(playerPos.X + moveSpeed) / 40] <= 0
                && level[((int)(playerPos.Y) / 40), (int)(playerPos.X + moveSpeed) / 40] <= 0)
                {
                    playerPos.X += moveSpeed;
                }

                //if the player WILL run into an obstacle background on the next frame, set the x position of
                //the player to the right hand side of the tile
                if (level[((int)(playerPos.Y + (playerHeight - 1)) / 40), (int)(playerPos.X + moveSpeed) / 40] > 0
                    || level[(int)(playerPos.Y / 40), (int)(playerPos.X + moveSpeed) / 40] > 0)
                {
                    //playerPos.X=  ((playerPos.X + moveSpeed)/40)+40;
                    //moveSpeed = 0.0f;

                    do
                    {
                        moveSpeed += 0.25f;
                    } while (level[((int)((playerPos.Y + playerHeight) - 1) / 40),
                        (int)(playerPos.X + moveSpeed) / 40] > 0
                        || level[(int)(playerPos.Y / 40), (int)(playerPos.X + moveSpeed) / 40] > 0);

                    playerPos.X += moveSpeed;
                    //playerPos.X += 1;

                    moveSpeed = 0.0f;


                }
            }

            //** Right>>>>>

            //press right key to add .25 from moveSpeed, make the character face right
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                moveSpeed += 0.25f;
                playerFacingRight = true;

            }

            //moving right
            if (moveSpeed > 0.0f)
            {

                movingRight = true;

                //if the player will NOT run into an obstacle background on the next frame, move the player
                if (level[((int)(playerPos.Y + playerHeight) - 1) / 40, (int)((playerPos.X + 40) + moveSpeed) / 40] <= 0
                && level[(int)(playerPos.Y) / 40, (int)((playerPos.X + 40) + moveSpeed) / 40] <= 0)
                {
                    playerPos.X += moveSpeed;
                }

                //if the player WILL run into an obstacle background on the next frame, subtract 0.25
                //from movespeed untill player will be perfectly to the left of the tile in next frame
                if (level[(int)(playerPos.Y + (playerHeight - 1)) / 40,
                    (int)((playerPos.X + playerWidth) + moveSpeed) / 40] > 0
                    || level[(int)playerPos.Y / 40,
                    (int)((playerPos.X + playerWidth) + moveSpeed) / 40] > 0)
                {


                    do
                    {
                        moveSpeed -= 0.25f;
                    } while (level[((int)(playerPos.Y + playerHeight) - 1) / 40,
                        (int)((playerPos.X + playerWidth) + moveSpeed) / 40] > 0
                        || level[(int)playerPos.Y / 40,
                    (int)((playerPos.X + playerWidth) + moveSpeed) / 40] > 0);

                    playerPos.X += moveSpeed;
                    playerPos.X += 1;

                    moveSpeed = 0.0f;

                }
            }


            //apply inertia to player character if both left and right keys are not being pressed
            if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right))
            {
                if (moveSpeed < 0.0f) //player is not pressing any keys & movespeed is less then zero
                {
                    moveSpeed += inertia;
                    if (moveSpeed > 0.0f)
                    {
                        moveSpeed = 0.0f;


                    }
                }

                if (moveSpeed > 0.0f)
                {
                    moveSpeed -= inertia;
                    if (moveSpeed < 0.0f)
                    {
                        moveSpeed = 0.0f;


                    }
                }
            }


            if (moveSpeed > 5.0f)
            {
                moveSpeed = 5.0f;
            }

            if (moveSpeed < -5.0f)
            {
                moveSpeed = -5.0f;
            }
        }

        private void doJumpFall()
        {
            //*** Fall ***

            //if neither of the players feet are touching the ground and the player is not jumping
            //make the player fall by adding to the gravity
            if (level[((int)(playerPos.Y + 40) / 40), (int)(playerPos.X) / 40] <= 0
                && level[((int)(playerPos.Y + 40) / 40), (int)(playerPos.X + (playerWidth - 1)) / 40] <= 0)
            {
                gravity += 0.25f;
                onGround = false;


            }

            //if the player will NOT fall inside an obstacle background on the next frame, make the player fall
            if (level[((int)((playerPos.Y + 40) + gravity) / 40), (int)(playerPos.X) / 40] <= 0
                && level[((int)((playerPos.Y + 40) + gravity) / 40), (int)(playerPos.X + (playerWidth - 1)) / 40] <= 0)
            {
                playerPos.Y += gravity;
            }


            //if on the next frame the player will end up inside the background, subtract
            //0.25 from the gravity until the player is flush with the ground


            if (level[((int)((playerPos.Y + 40) + gravity) / 40), (int)(playerPos.X) / 40] > 0
                        || level[((int)((playerPos.Y + 40) + gravity) / 40),
                        (int)(playerPos.X + (playerWidth - 1)) / 40] > 0)
            {
                do
                {
                    gravity -= 0.25f;

                } while (level[((int)((playerPos.Y + 40) + gravity) / 40), (int)(playerPos.X) / 40] > 0
                            || level[((int)((playerPos.Y + 40) + gravity) / 40),
                                (int)(playerPos.X + (playerWidth - 1)) / 40] > 0);





                playerPos.Y += gravity;
                playerPos.Y += 1;

                gravity = 0.0f;
                onGround = true;

            }





            // *** Jump ***

            //if the player is on the ground/not jumping,
            //pressing the control key will make the player jump
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) &&
                (level[((int)(playerPos.Y + 40) / 40), (int)(playerPos.X) / 40] > 0
                            || level[((int)(playerPos.Y + 40) / 40),
                                (int)(playerPos.X + (playerWidth - 1)) / 40] > 0))
            {
                gravity = -11.0f;
                jumping = true;
                mrstickFrameNumber = 18;
                jumpSfx.Play(sfxVol);

            }




            //if the player is traveling up & the player will end up inside a tile in the next frame,
            // 0.25 is added to gravity until the players head is flush with the tile above it
            //gravity is then set to 0.0f


            if (gravity < 0.0f) //player is traveling up
            {


                onGround = false;

                if (level[((int)((playerPos.Y) + gravity) / 40), (int)(playerPos.X) / 40] > 0
                || level[((int)((playerPos.Y) + gravity) / 40), (int)(playerPos.X + (playerWidth - 1)) / 40] > 0)
                {
                    do
                    {
                        gravity += 0.25f;
                    } while (level[((int)((playerPos.Y) + gravity) / 40), (int)(playerPos.X) / 40] > 0
                || level[((int)((playerPos.Y) + gravity) / 40), (int)(playerPos.X + (playerWidth - 1)) / 40] > 0);

                    //gravity = 0.0f;
                    //placehere

                    //onGround = false;
                    //mrstickFrameNumber = 23;

                }
            }

            //cap the gravity so the player doesent fall too fast
            if (gravity > 9.0f)
            {
                gravity = 9.0f;
            }



        }

        //private void doSlopes()
        //{

        //if (Keyboard.GetState().IsKeyDown(Keys.Z))
        //{

        //}
        //}

        void loadLevel(string levelToLoad)
        {



            System.IO.StreamReader levelFile1 = new System.IO.StreamReader("levels\\" + levelToLoad + ".txt");

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();


            //set the scrolling type of the level
            conversion_string = levelFile1.ReadLine();
            scrollingType = conversion_string;

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            //set the static background of the level-
            conversion_string = levelFile1.ReadLine();

            conversion_int = Convert.ToInt32(conversion_string);
            bgStaticToShow = conversion_int;


            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            //set the moving background of the level
            conversion_string = levelFile1.ReadLine();

            conversion_int = Convert.ToInt32(conversion_string);
            bgMovingToShow = conversion_int;

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            //load level tiles into map
            for (int y = 0; y < tilesDown; y++)
            {
                for (int x = 0; x < tilesAcross; x++)
                {
                    conversion_string = levelFile1.ReadLine();

                    conversion_int = Convert.ToInt32(conversion_string);
                    level[y, x] = conversion_int;
                }
            }

            //load coconut positions
            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 9; i++)
            {
                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                cocoNuts[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                cocoNuts[i].pos.Y = conversion_int;
            }

            //load flame positions
            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 20; i++)
            {
                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                flames[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                flames[i].pos.Y = conversion_int;
            }

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 50; i++)
            {
                //spikeUp

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                spikeUps[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                spikeUps[i].pos.Y = conversion_int;



            }

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 50; i++)
            {
                //spikeDown

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                spikeDowns[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                spikeDowns[i].pos.Y = conversion_int;



            }

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 10; i++)
            {
                //springMushroom

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                springMushrooms[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                springMushrooms[i].pos.Y = conversion_int;



            }

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 10; i++)
            {
                //bee

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                bees[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                bees[i].pos.Y = conversion_int;



            }

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 10; i++)
            {
                //bird

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                birds[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                birds[i].pos.Y = conversion_int;



            }

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 10; i++)
            {
                //duck

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                ducks[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                ducks[i].pos.Y = conversion_int;



            }

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 10; i++)
            {
                //fish

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                fishs[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                fishs[i].pos.Y = conversion_int;



            }

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 10; i++)
            {
                //hedgehog

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                hedgehogs[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                hedgehogs[i].pos.Y = conversion_int;



            }

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 10; i++)
            {
                //ladybird

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                ladybirds[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                ladybirds[i].pos.Y = conversion_int;



            }

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 10; i++)
            {
                //sheep

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                sheeps[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                sheeps[i].pos.Y = conversion_int;



            }

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 10; i++)
            {
                //snail

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                snails[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                snails[i].pos.Y = conversion_int;



            }

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 10; i++)
            {
                //turtle

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                turtles[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                turtles[i].pos.Y = conversion_int;



            }

            //load collectable positions

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 200; i++)
            {
                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                collectables[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                collectables[i].pos.Y = conversion_int;

            }

            //load 1up positions

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 3; i++)
            {
                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                oneUps[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                oneUps[i].pos.Y = conversion_int;

            }



            //load health up positions

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            for (int i = 0; i < 3; i++)
            {
                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                healthUps[i].pos.X = conversion_int;

                conversion_string = levelFile1.ReadLine();
                conversion_int = Convert.ToInt32(conversion_string);
                healthUps[i].pos.Y = conversion_int;

            }



            //load player start position

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            conversion_string = levelFile1.ReadLine();
            conversion_int = Convert.ToInt32(conversion_string);
            playerPos.X = conversion_int;

            conversion_string = levelFile1.ReadLine();
            conversion_int = Convert.ToInt32(conversion_string);
            playerPos.Y = conversion_int;



            //load level exit position

            //makes the level editor skip the tags that are in the level file
            random_string = levelFile1.ReadLine();

            conversion_string = levelFile1.ReadLine();
            conversion_int = Convert.ToInt32(conversion_string);
            levelExitPos.X = conversion_int;

            conversion_string = levelFile1.ReadLine();
            conversion_int = Convert.ToInt32(conversion_string);
            levelExitPos.Y = conversion_int;







            startOfLevel();


            //close the level file as were finished with it
            levelFile1.Close();


        }




    }




}
