using Clamamity.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamamity.Content
{
    [AutoloadBossHead]
    public class Head : ModNPC
    {
        NPC Hand1;
        NPC Hand2;
        bool HandsSpawned = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Flush");

            NPCID.Sets.NPCBestiaryDrawModifiers value = new(0)
            {
                // Influences how the NPC looks in the Bestiary
                Velocity = 1f // Draws the NPC in the bestiary as if its walking +1 tiles in the x	direction
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }
        public override void SetDefaults()
        {
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.boss = true;
            NPC.width = 128;
            NPC.height = 128;
            NPC.damage = 0;
            NPC.defense = 999;
            NPC.lifeMax = 1500;
            NPC.HitSound = SoundID.FemaleHit;
            NPC.DeathSound = SoundID.DD2_BetsyDeath;
            NPC.value = 60000f;
            NPC.knockBackResist = 0.2f;
            NPC.aiStyle = -1;
            BannerItem = Item.BannerToItem(Banner); // Makes kills of this NPC go towards dropping the banner it's associated with.
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Fuck bestiary, all my homies hate bestiary."),
            });
        }
        private void SpawnHands()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Because we want to spawn minions, and minions are NPCs, we have to do this on the server (or singleplayer, "!= NetmodeID.MultiplayerClient" covers both)
                // This means we also have to sync it after we spawned and set up the minion
                return;
            }

            var entitySource = NPC.GetSource_FromAI();
            for (int i = 0; i < 2; i++)
            {
                int index = NPC.NewNPC(entitySource, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<Hand>(), NPC.whoAmI);
                NPC minionNPC = Main.npc[index];

                // Now that the minion is spawned, we need to prepare it with data that is necessary for it to work
                // This is not required usually if you simply spawn NPCs, but because the minion is tied to the body, we need to pass this information to it
                if (i == 0)
                {
                    minionNPC.spriteDirection = -NPC.spriteDirection;
                    Hand1 = minionNPC;
                }
                else
                {
                    Hand2 = minionNPC;
                }

                if (minionNPC.ModNPC is Hand minion)
                {
                    // This checks if our spawned NPC is indeed the minion, and casts it so we can access its variables
                    minion.ParentIndex = NPC.whoAmI; // Let the minion know who the "parent" is
                    minion.PositionIndex = i; // Give it the iteration index so each minion has a separate one, used for movement
                    minionNPC.realLife = NPC.whoAmI;
                }

                // Finally, syncing, only sync on server and if the NPC actually exists (Main.maxNPCs is the index of a dummy NPC, there is no point syncing it)
                if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
                {
                }
            }
        }

        int PreviousAttackIndex = 1;
        int RandomAttack = 0;
        private int SetRandomAttackIndex() // Sets random attack index for both hands
        {
            while (PreviousAttackIndex == RandomAttack)
            {
                Random rnd = new();
                RandomAttack = (rnd.Next(2));
            }
            PreviousAttackIndex = RandomAttack;
            Main.NewText(RandomAttack); // Prints in chat the chosen attack index

            if (Hand1.ModNPC is Hand minion)
            {
                minion.AttackIndex = RandomAttack;
            }
            if (Hand2.ModNPC is Hand minion2)
            {
                minion2.AttackIndex = RandomAttack;
            }
            return RandomAttack;

        }

        readonly float a = 6f;
        readonly float b = 0.3f;
        public override void AI()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];

            if (HandsSpawned == false) // Spawns hands
            {
                SpawnHands();
                HandsSpawned = true;
            }

            if (Hand1.life <= 0 && Hand2.life <= 0) // When both hands die head becomes vulnurable
            {
                NPC.defense = 150;
            }

            if (NPC.ai[0] <= 0)         // Timer for the NPC, NPC.ai instead of variable
            {                           // because it auto synchronizes itself
                SetRandomAttackIndex(); // Also it adds the count every tick,
                NPC.ai[0] = 330f;         // in one second there is 60 ticks
            }
            else
            {
                NPC.ai[0] -= 1f;
            }

            FloatAbovePlayer();

            void FloatAbovePlayer()
            {
                Vector2 NPCCenter = new(NPC.Center.X, NPC.Center.Y);
                float c = player.Center.X - NPCCenter.X;   // Cordinates where the head will travel to
                float d = player.Center.Y - NPCCenter.Y - 300f; // -200 so it will be above the player
                float e = (float)Math.Sqrt(c * c + d * d);
                if (e < 30f)
                {
                    c = NPC.velocity.X;
                    d = NPC.velocity.Y;
                }
                else
                {
                    e = a / e;
                    c *= e * 1.8f;
                    d *= e * 1.3f;
                }
                if (NPC.velocity.X < c)
                {
                    NPC.velocity.X += b;
                    if (NPC.velocity.X < 0f && c > 0f)
                    {
                        NPC.velocity.X += b * 2f;
                    }
                }
                else if (NPC.velocity.X > c)
                {
                    NPC.velocity.X -= b;
                    if (NPC.velocity.X > 0f && c < 0f)
                    {
                        NPC.velocity.X -= b * 2f;
                    }
                }
                if (NPC.velocity.Y < d)
                {
                    NPC.velocity.Y += b;
                    if (NPC.velocity.Y < 0f && d > 0f)
                    {
                        NPC.velocity.Y += b * 2f;
                    }
                }
                else if (NPC.velocity.Y > d)
                {
                    NPC.velocity.Y -= b;
                    if (NPC.velocity.Y > 0f && d < 0f)
                    {
                        NPC.velocity.Y -= b * 2f;
                    }
                }
            }
            // Does not work
            // Because of the way ai works
            // TODO
            if (player.dead)
            {
                // If the targeted player is dead, flee
                NPC.velocity.Y -= 0.04f;
                // This method makes it so when the boss is in "despawn range" (outside of the screen), it despawns in 10 ticks
                NPC.EncourageDespawn(10);
                return;


            }
        }
        public override void OnKill()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
        }

    }
    internal class Hand : ModNPC

    {
        public int ParentIndex
        {
            get => (int)NPC.ai[0] - 1;
            set => NPC.ai[0] = value + 1;
        }

        public int PositionIndex
        {
            get => (int)NPC.ai[1] - 1;
            set => NPC.ai[1] = value + 1;
        }
        public int AttackIndex
        {
            get => (int)NPC.ai[2];
            set => NPC.ai[2] = value;
        }
        public override void SetStaticDefaults()
        {
            // By default enemies gain health and attack if hardmode is reached. this NPC should not be affected by that
            NPCID.Sets.DontDoHardmodeScaling[Type] = true;
            // Enemies can pick up coins, let's prevent it for this NPC
            NPCID.Sets.CantTakeLunchMoney[Type] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new(0)
            {
                Hide = true // Does not work, idk
                            // Todo
            };
        }

        public override void SetDefaults()
        {
            Main.npcFrameCount[NPC.type] = 2;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.direction = -1;
            NPC.width = 75;
            NPC.height = 75;
            NPC.damage = 7;
            NPC.defense = 25;
            NPC.lifeMax = 6000;
            NPC.HitSound = SoundID.DeerclopsHit;
            NPC.DeathSound = SoundID.DD2_OgreDeath;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0.8f;
            NPC.netAlways = true;
        }

        internal void SetFrame(int Frame)
        {
            NPC.frame.Y = 116 * Frame;      
        }
        internal void Teleport(Vector2 TeleportPosition)
        {
            NPC.position = TeleportPosition;
            for (int d = 0; d < 70; d++)
            {
                Dust.NewDust(NPC.position, NPC.width + 10, NPC.height + 10, DustID.PinkFairy, 0f, 0f, 150, default, 1.5f);
                NPC.alpha = 0;
            }
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        readonly float a = 6f;
        readonly float b = 0.3f;
        float c;
        int PreviousAttackIndex = 1;
        bool Teleported = false;
        public override void AI()
        {
            if (Main.npc[ParentIndex].life <= 0) // If the Head is dead kills itself
            {
                NPC.life = 0;
            }

            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }

            Player player = Main.player[NPC.target];
            Vector2 NpcPosition = NPC.Center;
            Vector2 VectorFromNpcToPlayer = NPC.position - player.position;
            Vector2 directionFromNpcToPlayer = VectorFromNpcToPlayer.SafeNormalize(Vector2.UnitX); directionFromNpcToPlayer = VectorFromNpcToPlayer.SafeNormalize(Vector2.UnitY);
            int projectiletype = ModContent.ProjectileType<PointingFingerProjectile>();
            var entitySource = NPC.GetSource_FromAI();

            var functionsMap = new Dictionary<int, Action> // Mapping out all possible attacks, better than cases IMO
            {
                { 0, Idle },
                { 1, Attack }
            };

            if (PreviousAttackIndex != AttackIndex)
            {
                NPC.ai[3] = 0f;
                NPC.alpha = 255;
                Teleported = false;
                for (int d = 0; d < 70; d++)
                {
                    Dust.NewDust(NPC.position, NPC.width + 10, NPC.height + 10, DustID.PinkFairy, 0f, 0f, 150, default, 1f);
                    NPC.alpha = 0;
                }
            }

            NPC.ai[3] += 1f;

            // Launches the attack
            functionsMap[AttackIndex].Invoke();

            PreviousAttackIndex = AttackIndex;

            // Fly to the center of the target	 
            void Attack()
            {
                if (NPC.ai[3] >= 60f)
                {
                    CrossAttack();
                }
                else
                {
                    NPC.velocity *= 0.8f;
                    NPC.alpha += 5;
                    Teleported = false;
                }
                if (NPC.ai[3] >= 100f)
                {
                    NPC.ai[3] = 0f;
                }
                SetFrame(1);
                NPC.knockBackResist = 0f;
            }

            void Idle()
            {
                if (!Teleported)
                {
                    Vector2 TeleportPosition = Main.npc[ParentIndex].position;

                    if (PositionIndex == 1)
                    {
                        TeleportPosition.X -= 200;
                    }
                    else
                    {
                        TeleportPosition.X += 200;
                    }
                    Teleport(TeleportPosition);
                    Teleported = true;
                }

                NPC.rotation = 0;
                SetFrame(0);
                NPC.knockBackResist = 0.8f;

                FloatParallelToHead();
            }

            void CrossAttack()
            {
                if (!Teleported)
                {
                    Vector2 TeleportPosition;
                    Vector2 ProjectilePostion;

                    TeleportPosition.Y = player.position.Y + player.velocity.Y * 30;
                    TeleportPosition.X = player.position.X + player.velocity.X * 30;
                    ProjectilePostion = TeleportPosition;

                    attackParticles(1, 300, TeleportPosition + Vector2.UnitY * 10);

                    if (NPC.lifeMax * 0.5f >= NPC.life)
                    {
                        Projectile.NewProjectile(entitySource, ProjectilePostion + Vector2.UnitY * 500, -Vector2.UnitY, projectiletype, 50, 0, Main.myPlayer);
                        Projectile.NewProjectile(entitySource, ProjectilePostion + Vector2.UnitY * -500, Vector2.UnitY, projectiletype, 50, 0, Main.myPlayer);
                    }

                    if (PositionIndex == 1)
                    {
                        NPC.velocity.X += 1f;
                        TeleportPosition.X -= 500;
                        NPC.rotation = MathHelper.ToRadians(90); // Turns degrees to radians
                    }
                    else
                    {
                        NPC.velocity.X -= 1f;
                        TeleportPosition.X += 500;
                        NPC.rotation = MathHelper.ToRadians(-90);
                    }

                    Teleport(TeleportPosition);
                    Teleported = true;
                }
                if (Math.Abs(NPC.velocity.X) <= 25)
                {
                    NPC.velocity.X *= 1.2f;
                }
            }

            void FloatParallelToHead()
            {
                Vector2 vector = new(NPC.Center.X, NPC.Center.Y);

                if (PositionIndex == 1)
                {
                    c = Main.npc[ParentIndex].Center.X - vector.X - 200f;
                }
                else
                {
                    c = Main.npc[ParentIndex].Center.X - vector.X + 200f;
                }
                float d = Main.npc[ParentIndex].Center.Y - vector.Y;
                float e = (float)Math.Sqrt(c * c + d * d);
                if (e < 20f)
                {
                    c = NPC.velocity.X;
                    d = NPC.velocity.Y;
                }
                else
                {
                    e = a / e;
                    c *= e * 2f;
                    d *= e * 1.5f;
                }
                if (NPC.velocity.X < c)
                {
                    NPC.velocity.X += b;
                    if (NPC.velocity.X < 0f && c > 0f)
                    {
                        NPC.velocity.X += b * 2f;
                    }
                }
                else if (NPC.velocity.X > c)
                {
                    NPC.velocity.X -= b;
                    if (NPC.velocity.X > 0f && c < 0f)
                    {
                        NPC.velocity.X -= b * 2f;
                    }
                }
                if (NPC.velocity.Y < d)
                {
                    NPC.velocity.Y += b;
                    if (NPC.velocity.Y < 0f && d > 0f)
                    {
                        NPC.velocity.Y += b * 2f;
                    }
                }
                else if (NPC.velocity.Y > d)
                {
                    NPC.velocity.Y -= b;
                    if (NPC.velocity.Y > 0f && d < 0f)
                    {
                        NPC.velocity.Y -= b * 2f;
                    }
                }

            }
            void attackParticles(int height,int width, Vector2 position)
            {
                Vector2 SpawnPos;

                SpawnPos.X = position.X - 0.5f * width;
                SpawnPos.Y = position.Y;
                for (int d = 0; d < 140; d++)
                {
                    int dustnumber = Dust.NewDust(SpawnPos , width, height, DustID.PinkFairy, 0f, 0f, 200, Color.OrangeRed, 1.2f);
                    Main.dust[dustnumber].noGravity = true;
                }

            }
        }
    }


}


