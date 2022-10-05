using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;
using System.Threading;
using System.Collections.Generic;
using Clamamity.Projectiles;


namespace Clamamity.Content
{
    public class FlushedMinion : ModNPC
    {
		public override void SetDefaults()
		{
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.width = 128;
			NPC.height = 128;
			NPC.damage = 0;
			NPC.defense = 10;
			NPC.lifeMax = 1500;
			NPC.HitSound = SoundID.FemaleHit;
			NPC.DeathSound = SoundID.DD2_BetsyDeath;
			NPC.value = 0f;
			NPC.knockBackResist = 0.2f;
			NPC.aiStyle = -1;
		}
        public override void AI()
        {
			bool flag24 = false;
			if (Main.expertMode && (double)NPC.life < (double)NPC.lifeMax * 0.12)
			{
				flag24 = true;
			}
			bool flag35 = false;
			if (Main.expertMode && (double)NPC.life < (double)NPC.lifeMax * 0.04)
			{
				flag35 = true;
			}
			float num926 = 20f;
			if (flag35)
			{
				num926 = 10f;
			}
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}
			bool dead = Main.player[NPC.target].dead;
			float num1037 = NPC.position.X + (float)(NPC.width / 2) - Main.player[NPC.target].position.X - (float)(Main.player[NPC.target].width / 2);
			float num1148 = NPC.position.Y + (float)NPC.height - 59f - Main.player[NPC.target].position.Y - (float)(Main.player[NPC.target].height / 2);
			float num1259 = (float)Math.Atan2(num1148, num1037) + 1.57f;
			if (num1259 < 0f)
			{
				num1259 += 6.283f;
			}
			else if ((double)num1259 > 6.283)
			{
				num1259 -= 6.283f;
			}
			float num1370 = 0f;
			if (NPC.ai[0] == 0f && NPC.ai[1] == 0f)
			{
				num1370 = 0.02f;
			}
			if (NPC.ai[0] == 0f && NPC.ai[1] == 2f && NPC.ai[2] > 40f)
			{
				num1370 = 0.05f;
			}
			if (NPC.ai[0] == 3f && NPC.ai[1] == 0f)
			{
				num1370 = 0.05f;
			}
			if (NPC.ai[0] == 3f && NPC.ai[1] == 2f && NPC.ai[2] > 40f)
			{
				num1370 = 0.08f;
			}
			if (NPC.ai[0] == 3f && NPC.ai[1] == 4f && NPC.ai[2] > num926)
			{
				num1370 = 0.15f;
			}
			if (NPC.ai[0] == 3f && NPC.ai[1] == 5f)
			{
				num1370 = 0.05f;
			}
			if (Main.expertMode)
			{
				num1370 *= 1.5f;
			}
			if (flag35 && Main.expertMode)
			{
				num1370 = 0f;
			}
			if (NPC.rotation < num1259)
			{
				if ((double)(num1259 - NPC.rotation) > 3.1415)
				{
					NPC.rotation -= num1370;
				}
				else
				{
					NPC.rotation += num1370;
				}
			}
			else if (NPC.rotation > num1259)
			{
				if ((double)(NPC.rotation - num1259) > 3.1415)
				{
					NPC.rotation += num1370;
				}
				else
				{
					NPC.rotation -= num1370;
				}
			}
			if (NPC.rotation > num1259 - num1370 && NPC.rotation < num1259 + num1370)
			{
				NPC.rotation = num1259;
			}
			if (NPC.rotation < 0f)
			{
				NPC.rotation += 6.283f;
			}
			else if ((double)NPC.rotation > 6.283)
			{
				NPC.rotation -= 6.283f;
			}
			if (NPC.rotation > num1259 - num1370 && NPC.rotation < num1259 + num1370)
			{
				NPC.rotation = num1259;
			}
			if (Main.rand.NextBool(5))
			{
				int num1481 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y + (float)NPC.height * 0.25f), NPC.width, (int)((float)NPC.height * 0.5f), DustID.Blood, NPC.velocity.X, 2f);
				Main.dust[num1481].velocity.X *= 0.5f;
				Main.dust[num1481].velocity.Y *= 0.1f;
			}
			if (dead)
			{
				NPC.velocity.Y -= 0.04f;
				NPC.EncourageDespawn(10);
				return;
			}
			if (NPC.ai[0] == 0f)
			{
				if (NPC.ai[1] == 0f)
				{
					float num2 = 5f;
					float num113 = 0.04f;
					if (Main.expertMode)
					{
						num113 = 0.15f;
						num2 = 7f;
					}
					if (Main.getGoodWorld)
					{
						num113 += 0.05f;
						num2 += 1f;
					}
					Vector2 vector = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					float num224 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector.X;
					float num335 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - 200f - vector.Y;
					float num446 = (float)Math.Sqrt(num224 * num224 + num335 * num335);
					float num557 = num446;
					num446 = num2 / num446;
					num224 *= num446;
					num335 *= num446;
					if (NPC.velocity.X < num224)
					{
						NPC.velocity.X += num113;
						if (NPC.velocity.X < 0f && num224 > 0f)
						{
							NPC.velocity.X += num113;
						}
					}
					else if (NPC.velocity.X > num224)
					{
						NPC.velocity.X -= num113;
						if (NPC.velocity.X > 0f && num224 < 0f)
						{
							NPC.velocity.X -= num113;
						}
					}
					if (NPC.velocity.Y < num335)
					{
						NPC.velocity.Y += num113;
						if (NPC.velocity.Y < 0f && num335 > 0f)
						{
							NPC.velocity.Y += num113;
						}
					}
					else if (NPC.velocity.Y > num335)
					{
						NPC.velocity.Y -= num113;
						if (NPC.velocity.Y > 0f && num335 < 0f)
						{
							NPC.velocity.Y -= num113;
						}
					}
					NPC.ai[2] += 1f;
					float num659 = 600f;
					if (Main.expertMode)
					{
						num659 *= 0.35f;
					}
					if (NPC.ai[2] >= num659)
					{
						NPC.ai[1] = 1f;
						NPC.ai[2] = 0f;
						NPC.ai[3] = 0f;
						NPC.target = 255;
						NPC.netUpdate = true;
					}
					else if ((NPC.position.Y + (float)NPC.height < Main.player[NPC.target].position.Y && num557 < 500f) || (Main.expertMode && num557 < 500f))
					{
						if (!Main.player[NPC.target].dead)
						{
							NPC.ai[3] += 1f;
						}
						float num670 = 110f;
						if (Main.expertMode)
						{
							num670 *= 0.4f;
						}
						if (Main.getGoodWorld)
						{
							num670 *= 0.8f;
						}
						if (NPC.ai[3] >= num670)
						{
							NPC.ai[3] = 0f;
							NPC.rotation = num1259;
							float num681 = 5f;
							if (Main.expertMode)
							{
								num681 = 6f;
							}
							float num692 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector.X;
							float num704 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector.Y;
							float num716 = (float)Math.Sqrt(num692 * num692 + num704 * num704);
							num716 = num681 / num716;
							Vector2 position = vector;
							Vector2 vector112 = default(Vector2);
							vector112.X = num692 * num716;
							vector112.Y = num704 * num716;
							position.X += vector112.X * 10f;
							position.Y += vector112.Y * 10f;
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int num727 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)position.X, (int)position.Y, 5);
								Main.npc[num727].velocity.X = vector112.X;
								Main.npc[num727].velocity.Y = vector112.Y;
								if (Main.netMode == NetmodeID.Server && num727 < 200)
								{
									NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num727);
								}
							}
							// SoundEngine.PlaySound(3, (int)position.X, (int)position.Y);
							for (int m = 0; m < 10; m++)
							{
								Dust.NewDust(position, 20, 20, DustID.Blood, vector112.X * 0.4f, vector112.Y * 0.4f);
							}
						}
					}
				}
				else if (NPC.ai[1] == 1f)
				{
					NPC.rotation = num1259;
					float num738 = 6f;
					if (Main.expertMode)
					{
						num738 = 7f;
					}
					if (Main.getGoodWorld)
					{
						num738 += 1f;
					}
					Vector2 vector165 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					float num749 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector165.X;
					float num760 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector165.Y;
					float num771 = (float)Math.Sqrt(num749 * num749 + num760 * num760);
					num771 = num738 / num771;
					NPC.velocity.X = num749 * num771;
					NPC.velocity.Y = num760 * num771;
					NPC.ai[1] = 2f;
					NPC.netUpdate = true;
					if (NPC.netSpam > 10)
					{
						NPC.netSpam = 10;
					}
				}
				else if (NPC.ai[1] == 2f)
				{
					NPC.ai[2] += 1f;
					if (NPC.ai[2] >= 40f)
					{
						NPC.velocity *= 0.98f;
						if (Main.expertMode)
						{
							NPC.velocity *= 0.985f;
						}
						if (Main.getGoodWorld)
						{
							NPC.velocity *= 0.99f;
						}
						if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
						{
							NPC.velocity.X = 0f;
						}
						if ((double)NPC.velocity.Y > -0.1 && (double)NPC.velocity.Y < 0.1)
						{
							NPC.velocity.Y = 0f;
						}
					}
					else
					{
						NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
					}
					int num782 = 150;
					if (Main.expertMode)
					{
						num782 = 100;
					}
					if (Main.getGoodWorld)
					{
						num782 -= 15;
					}
					if (NPC.ai[2] >= (float)num782)
					{
						NPC.ai[3] += 1f;
						NPC.ai[2] = 0f;
						NPC.target = 255;
						NPC.rotation = num1259;
						if (NPC.ai[3] >= 3f)
						{
							NPC.ai[1] = 0f;
							NPC.ai[3] = 0f;
						}
						else
						{
							NPC.ai[1] = 1f;
						}
					}
				}
				float num793 = 0.5f;
				if (Main.expertMode)
				{
					num793 = 0.65f;
				}
				if ((float)NPC.life < (float)NPC.lifeMax * num793)
				{
					NPC.ai[0] = 1f;
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					NPC.netUpdate = true;
					if (NPC.netSpam > 10)
					{
						NPC.netSpam = 10;
					}
				}
				return;
			}
			if (NPC.ai[0] == 1f || NPC.ai[0] == 2f)
			{
				if (NPC.ai[0] == 1f)
				{
					NPC.ai[2] += 0.005f;
					if ((double)NPC.ai[2] > 0.5)
					{
						NPC.ai[2] = 0.5f;
					}
				}
				else
				{
					NPC.ai[2] -= 0.005f;
					if (NPC.ai[2] < 0f)
					{
						NPC.ai[2] = 0f;
					}
				}
				NPC.rotation += NPC.ai[2];
				NPC.ai[1] += 1f;
				if (Main.expertMode && NPC.ai[1] % 20f == 0f)
				{
					float num804 = 5f;
					Vector2 vector176 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					float num816 = Main.rand.Next(-200, 200);
					float num827 = Main.rand.Next(-200, 200);
					float num838 = (float)Math.Sqrt(num816 * num816 + num827 * num827);
					num838 = num804 / num838;
					Vector2 position3 = vector176;
					Vector2 vector187 = default(Vector2);
					vector187.X = num816 * num838;
					vector187.Y = num827 * num838;
					position3.X += vector187.X * 10f;
					position3.Y += vector187.Y * 10f;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int num849 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)position3.X, (int)position3.Y, 5);
						Main.npc[num849].velocity.X = vector187.X;
						Main.npc[num849].velocity.Y = vector187.Y;
						if (Main.netMode == NetmodeID.Server && num849 < 200)
						{
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num849);
						}
					}
					for (int n = 0; n < 10; n++)
					{
						Dust.NewDust(position3, 20, 20, DustID.Blood, vector187.X * 0.4f, vector187.Y * 0.4f);
					}
				}
				if (NPC.ai[1] >= 100f)
				{
					NPC.ai[0] += 1f;
					NPC.ai[1] = 0f;
					if (NPC.ai[0] == 3f)
					{
						NPC.ai[2] = 0f;
					}
					else
					{
						// SoundEngine.PlaySound(3, (int)NPC.position.X, (int)NPC.position.Y);
						for (int num860 = 0; num860 < 2; num860++)
						{
							// Gore.NewGore(NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 8);
							// Gore.NewGore(NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 7);
							// Gore.NewGore(NPC.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 6);
						}
						for (int num871 = 0; num871 < 20; num871++)
						{
							Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
						}
						// SoundEngine.PlaySound(15, (int)NPC.position.X, (int)NPC.position.Y, 0);
					}
				}
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
				NPC.velocity.X *= 0.98f;
				NPC.velocity.Y *= 0.98f;
				if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
				{
					NPC.velocity.X = 0f;
				}
				if ((double)NPC.velocity.Y > -0.1 && (double)NPC.velocity.Y < 0.1)
				{
					NPC.velocity.Y = 0f;
				}
				return;
			}
			NPC.defense = 0;
			int num882 = 23;
			int num893 = 18;
			if (Main.expertMode)
			{
				if (flag24)
				{
					NPC.defense = -15;
				}
				if (flag35)
				{
					num893 = 20;
					NPC.defense = -30;
				}
			}
			NPC.damage = NPC.GetAttackDamage_LerpBetweenFinalValues(num882, num893);
			NPC.damage = NPC.GetAttackDamage_ScaledByStrength(NPC.damage);
			if (NPC.ai[1] == 0f && flag24)
			{
				NPC.ai[1] = 5f;
			}
			if (NPC.ai[1] == 0f)
			{
				float num904 = 6f;
				float num915 = 0.07f;
				Vector2 vector198 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num927 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector198.X;
				float num938 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - 120f - vector198.Y;
				float num949 = (float)Math.Sqrt(num927 * num927 + num938 * num938);
				if (num949 > 400f && Main.expertMode)
				{
					num904 += 1f;
					num915 += 0.05f;
					if (num949 > 600f)
					{
						num904 += 1f;
						num915 += 0.05f;
						if (num949 > 800f)
						{
							num904 += 1f;
							num915 += 0.05f;
						}
					}
				}
				if (Main.getGoodWorld)
				{
					num904 += 1f;
					num915 += 0.1f;
				}
				num949 = num904 / num949;
				num927 *= num949;
				num938 *= num949;
				if (NPC.velocity.X < num927)
				{
					NPC.velocity.X += num915;
					if (NPC.velocity.X < 0f && num927 > 0f)
					{
						NPC.velocity.X += num915;
					}
				}
				else if (NPC.velocity.X > num927)
				{
					NPC.velocity.X -= num915;
					if (NPC.velocity.X > 0f && num927 < 0f)
					{
						NPC.velocity.X -= num915;
					}
				}
				if (NPC.velocity.Y < num938)
				{
					NPC.velocity.Y += num915;
					if (NPC.velocity.Y < 0f && num938 > 0f)
					{
						NPC.velocity.Y += num915;
					}
				}
				else if (NPC.velocity.Y > num938)
				{
					NPC.velocity.Y -= num915;
					if (NPC.velocity.Y > 0f && num938 < 0f)
					{
						NPC.velocity.Y -= num915;
					}
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= 200f)
				{
					NPC.ai[1] = 1f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					if (Main.expertMode && (double)NPC.life < (double)NPC.lifeMax * 0.35)
					{
						NPC.ai[1] = 3f;
					}
					NPC.target = 255;
					NPC.netUpdate = true;
				}
				if (Main.expertMode && flag35)
				{
					NPC.TargetClosest();
					NPC.netUpdate = true;
					NPC.ai[1] = 3f;
					NPC.ai[2] = 0f;
					NPC.ai[3] -= 1000f;
				}
			}
			else if (NPC.ai[1] == 1f)
			{
				// SoundEngine.PlaySound(36, (int)NPC.position.X, (int)NPC.position.Y, 0);
				NPC.rotation = num1259;
				float num960 = 6.8f;
				if (Main.expertMode && NPC.ai[3] == 1f)
				{
					num960 *= 1.15f;
				}
				if (Main.expertMode && NPC.ai[3] == 2f)
				{
					num960 *= 1.3f;
				}
				if (Main.getGoodWorld)
				{
					num960 *= 1.2f;
				}
				Vector2 vector209 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num971 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector209.X;
				float num982 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector209.Y;
				float num993 = (float)Math.Sqrt(num971 * num971 + num982 * num982);
				num993 = num960 / num993;
				NPC.velocity.X = num971 * num993;
				NPC.velocity.Y = num982 * num993;
				NPC.ai[1] = 2f;
				NPC.netUpdate = true;
				if (NPC.netSpam > 10)
				{
					NPC.netSpam = 10;
				}
			}
			else if (NPC.ai[1] == 2f)
			{
				float num1004 = 40f;
				NPC.ai[2] += 1f;
				if (Main.expertMode)
				{
					num1004 = 50f;
				}
				if (NPC.ai[2] >= num1004)
				{
					NPC.velocity *= 0.97f;
					if (Main.expertMode)
					{
						NPC.velocity *= 0.98f;
					}
					if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
					{
						NPC.velocity.X = 0f;
					}
					if ((double)NPC.velocity.Y > -0.1 && (double)NPC.velocity.Y < 0.1)
					{
						NPC.velocity.Y = 0f;
					}
				}
				else
				{
					NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
				}
				int num1015 = 130;
				if (Main.expertMode)
				{
					num1015 = 90;
				}
				if (NPC.ai[2] >= (float)num1015)
				{
					NPC.ai[3] += 1f;
					NPC.ai[2] = 0f;
					NPC.target = 255;
					NPC.rotation = num1259;
					if (NPC.ai[3] >= 3f)
					{
						NPC.ai[1] = 0f;
						NPC.ai[3] = 0f;
						if (Main.expertMode && Main.netMode != NetmodeID.MultiplayerClient && (double)NPC.life < (double)NPC.lifeMax * 0.5)
						{
							NPC.ai[1] = 3f;
							NPC.ai[3] += Main.rand.Next(1, 4);
						}
						NPC.netUpdate = true;
						if (NPC.netSpam > 10)
						{
							NPC.netSpam = 10;
						}
					}
					else
					{
						NPC.ai[1] = 1f;
					}
				}
			}
			else if (NPC.ai[1] == 3f)
			{
				if (NPC.ai[3] == 4f && flag24 && NPC.Center.Y > Main.player[NPC.target].Center.Y)
				{
					NPC.TargetClosest();
					NPC.ai[1] = 0f;
					NPC.ai[2] = 0f;
					NPC.ai[3] = 0f;
					NPC.netUpdate = true;
					if (NPC.netSpam > 10)
					{
						NPC.netSpam = 10;
					}
				}
				else if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.TargetClosest();
					float num1026 = 20f;
					Vector2 vector220 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
					float num1038 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector220.X;
					float num1049 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector220.Y;
					float num1060 = Math.Abs(Main.player[NPC.target].velocity.X) + Math.Abs(Main.player[NPC.target].velocity.Y) / 4f;
					num1060 += 10f - num1060;
					if (num1060 < 5f)
					{
						num1060 = 5f;
					}
					if (num1060 > 15f)
					{
						num1060 = 15f;
					}
					if (NPC.ai[2] == -1f && !flag35)
					{
						num1060 *= 4f;
						num1026 *= 1.3f;
					}
					if (flag35)
					{
						num1060 *= 2f;
					}
					num1038 -= Main.player[NPC.target].velocity.X * num1060;
					num1049 -= Main.player[NPC.target].velocity.Y * num1060 / 4f;
					num1038 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					num1049 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					if (flag35)
					{
						num1038 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
						num1049 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					}
					float num1071 = (float)Math.Sqrt(num1038 * num1038 + num1049 * num1049);
					float num1082 = num1071;
					num1071 = num1026 / num1071;
					NPC.velocity.X = num1038 * num1071;
					NPC.velocity.Y = num1049 * num1071;
					NPC.velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
					NPC.velocity.Y += (float)Main.rand.Next(-20, 21) * 0.1f;
					if (flag35)
					{
						NPC.velocity.X += (float)Main.rand.Next(-50, 51) * 0.1f;
						NPC.velocity.Y += (float)Main.rand.Next(-50, 51) * 0.1f;
						float num1093 = Math.Abs(NPC.velocity.X);
						float num1104 = Math.Abs(NPC.velocity.Y);
						if (NPC.Center.X > Main.player[NPC.target].Center.X)
						{
							num1104 *= -1f;
						}
						if (NPC.Center.Y > Main.player[NPC.target].Center.Y)
						{
							num1093 *= -1f;
						}
						NPC.velocity.X = num1104 + NPC.velocity.X;
						NPC.velocity.Y = num1093 + NPC.velocity.Y;
						NPC.velocity.Normalize();
						NPC.velocity *= num1026;
						NPC.velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
						NPC.velocity.Y += (float)Main.rand.Next(-20, 21) * 0.1f;
					}
					else if (num1082 < 100f)
					{
						if (Math.Abs(NPC.velocity.X) > Math.Abs(NPC.velocity.Y))
						{
							float num1115 = Math.Abs(NPC.velocity.X);
							float num1126 = Math.Abs(NPC.velocity.Y);
							if (NPC.Center.X > Main.player[NPC.target].Center.X)
							{
								num1126 *= -1f;
							}
							if (NPC.Center.Y > Main.player[NPC.target].Center.Y)
							{
								num1115 *= -1f;
							}
							NPC.velocity.X = num1126;
							NPC.velocity.Y = num1115;
						}
					}
					else if (Math.Abs(NPC.velocity.X) > Math.Abs(NPC.velocity.Y))
					{
						float num1137 = (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y)) / 2f;
						float num1149 = num1137;
						if (NPC.Center.X > Main.player[NPC.target].Center.X)
						{
							num1149 *= -1f;
						}
						if (NPC.Center.Y > Main.player[NPC.target].Center.Y)
						{
							num1137 *= -1f;
						}
						NPC.velocity.X = num1149;
						NPC.velocity.Y = num1137;
					}
					NPC.ai[1] = 4f;
					NPC.netUpdate = true;
					if (NPC.netSpam > 10)
					{
						NPC.netSpam = 10;
					}
				}
			}
			else if (NPC.ai[1] == 4f)
			{
				if (NPC.ai[2] == 0f)
				{
					// SoundEngine.PlaySound(36, (int)NPC.position.X, (int)NPC.position.Y, -1);
				}
				float num1160 = num926;
				NPC.ai[2] += 1f;
				if (NPC.ai[2] == num1160 && Vector2.Distance(NPC.position, Main.player[NPC.target].position) < 200f)
				{
					NPC.ai[2] -= 1f;
				}
				if (NPC.ai[2] >= num1160)
				{
					NPC.velocity *= 0.95f;
					if ((double)NPC.velocity.X > -0.1 && (double)NPC.velocity.X < 0.1)
					{
						NPC.velocity.X = 0f;
					}
					if ((double)NPC.velocity.Y > -0.1 && (double)NPC.velocity.Y < 0.1)
					{
						NPC.velocity.Y = 0f;
					}
				}
				else
				{
					NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) - 1.57f;
				}
				float num1171 = num1160 + 13f;
				if (NPC.ai[2] >= num1171)
				{
					NPC.netUpdate = true;
					if (NPC.netSpam > 10)
					{
						NPC.netSpam = 10;
					}
					NPC.ai[3] += 1f;
					NPC.ai[2] = 0f;
					if (NPC.ai[3] >= 5f)
					{
						NPC.ai[1] = 0f;
						NPC.ai[3] = 0f;
					}
					else
					{
						NPC.ai[1] = 3f;
					}
				}
			}
			else if (NPC.ai[1] == 5f)
			{
				float num1182 = 600f;
				float num1193 = 9f;
				float num1204 = 0.3f;
				Vector2 vector231 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
				float num1215 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector231.X;
				float num1226 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) + num1182 - vector231.Y;
				float num1237 = (float)Math.Sqrt(num1215 * num1215 + num1226 * num1226);
				num1237 = num1193 / num1237;
				num1215 *= num1237;
				num1226 *= num1237;
				if (NPC.velocity.X < num1215)
				{
					NPC.velocity.X += num1204;
					if (NPC.velocity.X < 0f && num1215 > 0f)
					{
						NPC.velocity.X += num1204;
					}
				}
				else if (NPC.velocity.X > num1215)
				{
					NPC.velocity.X -= num1204;
					if (NPC.velocity.X > 0f && num1215 < 0f)
					{
						NPC.velocity.X -= num1204;
					}
				}
				if (NPC.velocity.Y < num1226)
				{
					NPC.velocity.Y += num1204;
					if (NPC.velocity.Y < 0f && num1226 > 0f)
					{
						NPC.velocity.Y += num1204;
					}
				}
				else if (NPC.velocity.Y > num1226)
				{
					NPC.velocity.Y -= num1204;
					if (NPC.velocity.Y > 0f && num1226 < 0f)
					{
						NPC.velocity.Y -= num1204;
					}
				}
				NPC.ai[2] += 1f;
				if (NPC.ai[2] >= 70f)
				{
					NPC.TargetClosest();
					NPC.ai[1] = 3f;
					NPC.ai[2] = -1f;
					NPC.ai[3] = Main.rand.Next(-3, 1);
					NPC.netUpdate = true;
				}
			}
			if (flag35 && NPC.ai[1] == 5f)
			{
				NPC.ai[1] = 3f;
			}
			return;
		}
	}
}

