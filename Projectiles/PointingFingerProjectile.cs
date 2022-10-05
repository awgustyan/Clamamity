using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Clamamity.Projectiles
{
	// This file shows an animated projectile
	// This file also shows advanced drawing to center the drawn projectile correctly
	public class PointingFingerProjectile : ModProjectile
	{	
		public override void SetDefaults()
		{
			Projectile.width = 70; // The width of projectile hitbox
			Projectile.height = 70; // The height of projectile hitbox

			Projectile.friendly = false; // Can the projectile deal damage to enemies?
			Projectile.damage = 50;
			Projectile.hostile = true;
			Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false; // Can the projectile collide with tiles?
			Projectile.penetrate = -1; // Look at comments ExamplePiercingProjectile

			Projectile.alpha = 255; // How transparent to draw this projectile. 0 to 255. 255 is completely transparent.
		}

		// Allows you to determine the color and transparency in which a projectile is drawn
		// Return null to use the default color (normally light and buff color)
		// Returns null by default.
		public override Color? GetAlpha(Color lightColor)
		{
			// return Color.White;
			return new Color(255, 255, 255, 0) * Projectile.Opacity;
		}

		public override void AI()
		{
			DrawOriginOffsetY = -45;
			if (Projectile.ai[0] == 0)
			{
				for (int d = 0; d < 70; d++)
				{
					Dust.NewDust(Projectile.position, Projectile.width + 10, Projectile.height + 10, DustID.PinkFairy, 0f, 0f, 150, default, 1f);
					Projectile.alpha = 0;
				}
			}
			// All projectiles have timers that help to delay certain events
			// Projectile.ai[0], Projectile.ai[1] — timers that are automatically synchronized on the client and server
			// Projectile.localAI[0], Projectile.localAI[0] — only on the client
			// In this example, a timer is used to control the fade in / out and despawn of the projectile
			Projectile.ai[0] += 1f;

			// Slow down
			if (Projectile.ai[0] >= 60f)
            {
				Projectile.velocity *= 0.8f;
				Projectile.alpha += 5;
			}

			if (Math.Abs(Projectile.velocity.Y) <= 25 && Projectile.ai[0] <= 60f)
			{
				Projectile.velocity.Y *= 1.2f;
			}

			// Set both direction and spriteDirection to 1 or -1 (right and left respectively)
			// Projectile.direction is automatically set correctly in Projectile.Update, but we need to set it here or the textures will draw incorrectly on the 1st frame.
			Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;

			Projectile.rotation = Projectile.velocity.ToRotation();
			// Since our sprite has an orientation, we need to adjust rotation to compensate for the draw flipping

			Projectile.rotation += MathHelper.PiOver2;
			// For vertical sprites use MathHelper.PiOver2
			if (Projectile.ai[0] >= 80f)
				Projectile.Kill();
		}
	}
}
