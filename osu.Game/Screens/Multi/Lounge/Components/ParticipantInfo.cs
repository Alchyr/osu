﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Online.Chat;
using osu.Game.Users;
using osuTK;

namespace osu.Game.Screens.Multi.Lounge.Components
{
    public class ParticipantInfo : Container
    {
        private readonly FillFlowContainer levelRangeContainer;

        public readonly Bindable<User> Host = new Bindable<User>();
        public readonly Bindable<IEnumerable<User>> Participants = new Bindable<IEnumerable<User>>();

        public ParticipantInfo(string rankPrefix = null)
        {
            RelativeSizeAxes = Axes.X;
            Height = 15f;

            OsuSpriteText levelRangeHigher;
            OsuSpriteText levelRangeLower;
            Container flagContainer;
            LinkFlowContainer hostText;

            Children = new Drawable[]
            {
                new FillFlowContainer
                {
                    AutoSizeAxes = Axes.X,
                    RelativeSizeAxes = Axes.Y,
                    Direction = FillDirection.Horizontal,
                    Spacing = new Vector2(5f, 0f),
                    Children = new Drawable[]
                    {
                        flagContainer = new Container
                        {
                            Width = 22f,
                            RelativeSizeAxes = Axes.Y,
                        },
                        new Container //todo: team banners
                        {
                            Width = 38f,
                            RelativeSizeAxes = Axes.Y,
                            CornerRadius = 2f,
                            Masking = true,
                            Children = new[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = OsuColour.FromHex(@"ad387e"),
                                },
                            },
                        },
                        hostText = new LinkFlowContainer
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            AutoSizeAxes = Axes.Both
                        }
                    },
                },
                levelRangeContainer = new FillFlowContainer
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Children = new[]
                    {
                        new OsuSpriteText
                        {
                            Text = rankPrefix,
                            TextSize = 14,
                        },
                        new OsuSpriteText
                        {
                            Text = "#",
                            TextSize = 14,
                        },
                        levelRangeLower = new OsuSpriteText
                        {
                            TextSize = 14,
                            Font = @"Exo2.0-Bold",
                        },
                        new OsuSpriteText
                        {
                            Text = " - ",
                            TextSize = 14,
                        },
                        new OsuSpriteText
                        {
                            Text = "#",
                            TextSize = 14,
                        },
                        levelRangeHigher = new OsuSpriteText
                        {
                            TextSize = 14,
                            Font = @"Exo2.0-Bold",
                        },
                    },
                },
            };

            Host.BindValueChanged(v =>
            {
                hostText.Clear();
                hostText.AddText("hosted by ");
                hostText.AddLink(v.Username, null, LinkAction.OpenUserProfile, v.Id.ToString(), "Open profile", s => s.Font = "Exo2.0-BoldItalic");

                flagContainer.Child = new DrawableFlag(v.Country) { RelativeSizeAxes = Axes.Both };
            });

            Participants.BindValueChanged(v =>
            {
                var ranks = v.Select(u => u.Statistics.Ranks.Global);
                levelRangeLower.Text = ranks.Min().ToString();
                levelRangeHigher.Text = ranks.Max().ToString();
            });
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            levelRangeContainer.Colour = colours.Gray9;
        }
    }
}
