using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Spells.Chivalry
{
	public class CleanseByFireSpell : PaladinSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Cleanse By Fire", "Expor Flamus",
				-1,
				9002
			);

		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 1.0 ); } }

		public override double RequiredSkill{ get{ return 5.0; } }
		public override int RequiredMana{ get{ return 10; } }
		public override int RequiredTithing{ get{ return 10; } }
		public override int MantraNumber{ get{ return 1060718; } } // Expor Flamus

		public CleanseByFireSpell( Mobile caster, Item scroll ) : base( caster, scroll, m_Info )
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !m.Poisoned )
			{
				Caster.SendLocalizedMessage( 1060176 ); // That creature is not poisoned!
			}
			else if ( CheckBSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

				/* Cures the target of poisons, but causes the caster to be burned by fire damage for 13-55 hit points.
				 * The amount of fire damage is lessened if the caster has high Karma.
				 */

				Poison p = m.Poison;

				if ( p != null )
				{
					// Cleanse by fire is now difficulty based 
					int chanceToCure = 10000 + (int)(Caster.Skills[SkillName.Knightship].Value * 75) - ((p.Level + 1) * 2000);
					chanceToCure /= 100;

					if ( chanceToCure > Utility.Random( 100 ) )
					{
						if ( m.CurePoison( Caster ) )
						{
							if ( Caster != m )
								Caster.SendLocalizedMessage( 1010058 ); // You have cured the target of all poisons!

							m.SendLocalizedMessage( 1010059 ); // You have been cured of all poisons.
						}
					}
					else
					{
						m.SendLocalizedMessage( 1010060 ); // You have failed to cure your target!
					}
				}

				m.PlaySound( 0x1E0 );
				m.FixedParticles( 0x373A, 1, 15, 5012, 3, 2, EffectLayer.Waist );

				IEntity from = new Entity( Serial.Zero, new Point3D( m.X, m.Y, m.Z - 5 ), m.Map );
				IEntity to = new Entity( Serial.Zero, new Point3D( m.X, m.Y, m.Z + 45 ), m.Map );
				Effects.SendMovingParticles( from, to, 0x374B, 1, 0, false, false, 63, 2, 9501, 1, 0, EffectLayer.Head, 0x100 );

				Caster.PlaySound( 0x208 );
				Caster.FixedParticles( 0x3709, 1, 30, 9934, 0, 7, EffectLayer.Waist );

				int damage = 50 - ComputePowerValue( 4 );

				// TODO: Should caps be applied?
				if ( damage < 13 )
					damage = 13;
				else if ( damage > 55 )
					damage = 55;

				AOS.Damage( Caster, Caster, damage, 0, 100, 0, 0, 0, true );
			}

			FinishSequence();
		}

        public void Target(Item i)
        {
			if (i is BaseBeverage)
			{
				BaseBeverage bev = (BaseBeverage)i;
				if (bev.Quantity > 0)
				{
					if (bev.Poison == null)
					{
						Caster.SendMessage("That beverage is not poisoned!");
					}
					else if (CheckSequence())
					{
						SpellHelper.Turn(Caster, i);

						/* Cures the target beverage of poisons, but causes a portion of it to be lost.
                         * The amount of loss is lessened if the caster has high Karma.
                         */

						Poison p = bev.Poison;

						// Cleanse by fire is now difficulty based 
						int chanceToCure = 10000 + (int)(Caster.Skills[SkillName.Knightship].Value * 75) - ((p.Level + 1) * 2000);
						chanceToCure /= 100;

						if (chanceToCure > Utility.Random(100))
						{
							bev.Poison = null;
							bev.Poisoner = null;

							Caster.SendMessage("You have purified the beverage of poison, boiling some of it away in the process.");
						}
						else
						{
							Caster.SendMessage("You have failed to purify your target, boiling some of it away in the process!"); // You have failed to cure your target!
						}

						IEntity from = new Entity(Serial.Zero, new Point3D(i.X, i.Y, i.Z - 5), i.Map);
						IEntity to = new Entity(Serial.Zero, new Point3D(i.X, i.Y, i.Z + 45), i.Map);
						Effects.SendMovingParticles(from, to, 0x374B, 1, 0, false, false, 63, 2, 9501, 1, 0, EffectLayer.Head, 0x100);

						Caster.PlaySound(0x208);
						Caster.FixedParticles(0x3709, 1, 30, 9934, 0, 7, EffectLayer.Waist);


						int reduction = 3 - ComputePowerValue(50);
						if (reduction < 1)
							reduction = 1;
						else if (reduction > 5)
							reduction = 5;
						bev.Quantity -= reduction;

						if (bev.Quantity == 0)
							Caster.SendMessage("You boiled away the last of the beverage!");
					}
				}
				else
					Caster.SendMessage("This container is empty!");
			}
			else if (i is Food)
			{
				Food food = (Food)i;
                if (food.Poison == null)
                {
                    Caster.SendMessage("That food is not poisoned!");
                }
				else if (CheckSequence())
				{
                    SpellHelper.Turn(Caster, i);

                    /* Cures the target beverage of poisons, but causes a portion of it to be lost.
                     * The amount of loss is lessened if the caster has high Karma.
                     */

                    Poison p = food.Poison;

                    // Cleanse by fire is now difficulty based 
                    int chanceToCure = 10000 + (int)(Caster.Skills[SkillName.Knightship].Value * 75) - ((p.Level + 1) * 2000);
                    chanceToCure /= 100;

					// food is purified individually, destroying one each time you fail
					if (chanceToCure > Utility.Random(100))
                    {
                        food.Poison = null;
                        food.Poisoner = null;

                        Caster.SendMessage("You have purified the food of poison.");
                    }
                    else
                    {
						food.Amount -= 1;
                        Caster.SendMessage("You have failed to purify your target, burning it in the process!"); // You have failed to cure your target!
                    }
                }
            }
			else
				Caster.SendMessage("You can only purify food and beverages!");

            FinishSequence();
        }

        private class InternalTarget : Target
		{
			private CleanseByFireSpell m_Owner;

			public InternalTarget( CleanseByFireSpell owner ) : base( Core.ML ? 10 : 12, false, TargetFlags.Beneficial )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if (o is Mobile)
					m_Owner.Target((Mobile)o);
				else if (o is Item)
					m_Owner.Target((Item)o);
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}