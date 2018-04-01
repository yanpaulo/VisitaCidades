using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VisitaCidades.Model;

namespace VisitaCidades
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont mainFont;
        private SpriteFont captionFont;
        private Texture2D pointer;
        private Texture2D bg;
        private Texture2D color;

        private IAlgoritmo algoritmo;

        public Game1(IAlgoritmo algoritmo)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 600;
            IsMouseVisible = true;
            //Window.AllowUserResizing = true;

            this.algoritmo = algoritmo;
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
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            mainFont = Content.Load<SpriteFont>("Font/Main");
            captionFont = Content.Load<SpriteFont>("Font/Caption");
            pointer = Content.Load<Texture2D>("Images/hand-pointer-small");

            bg = new Texture2D(GraphicsDevice, algoritmo.Problema.Mapa.Tamanho.Width, algoritmo.Problema.Mapa.Tamanho.Height);
            color = new Texture2D(GraphicsDevice, 10, 10);

            bg.SetData(Enumerable.Range(0, bg.Width * bg.Height).Select(n => Color.White).ToArray());
            color.SetData(Enumerable.Range(0, color.Width * color.Height).Select(n => Color.White).ToArray());

            algoritmo.ExecutaAsync();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var mousePosition = Mouse.GetState().Position.ToVector2();

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(bg, algoritmo.Problema.Mapa.Tamanho, Color.White);


            foreach (var local in algoritmo.Problema.Mapa.Locais)
            {
                if (Vector2.Distance(local.Posicao, mousePosition) < 50)
                {
                    spriteBatch.DrawString(captionFont, $"{local.Nome}{local.Posicao}", local.Posicao + new Vector2(0, -40), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(captionFont, local.Nome, local.Posicao + new Vector2(0, -20), Color.Gray);
                }
                spriteBatch.Draw(color, local.Posicao, Color.Blue);
            }

            if (algoritmo.Solucao != null)
            {
                foreach (var rota in algoritmo.Solucao.Rotas)
                {
                    spriteBatch.DrawString(captionFont, rota.Viajante.Nome, rota.Locais.First().Posicao - new Vector2(-10, 0), Color.Black);

                    for (int i = 0; i < rota.Locais.Count - 1; i++)
                    {
                        var atual = rota.Locais[i];
                        var proximo = rota.Locais[i + 1];

                        var direcao = proximo.Posicao - atual.Posicao;
                        var distancia = Vector2.Distance(atual.Posicao, proximo.Posicao);
                        var angulo = (float)Math.Atan2(direcao.Y, direcao.X);
                        var meio = atual.Posicao + Vector2.Multiply(direcao, 0.5f);

                        if (Vector2.Distance(meio, mousePosition) < 200)
                        {
                            spriteBatch.DrawString(mainFont, distancia.ToString("N"), meio + new Vector2(0, 10), Color.DarkGray, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
                        }
                        if (Vector2.Distance(atual.Posicao, mousePosition) < 50)
                        {
                            spriteBatch.Draw(pointer, atual.Posicao, null, Color.White, angulo, Vector2.Zero, 0.7f, SpriteEffects.FlipHorizontally, 1f);
                        }
                        spriteBatch.Draw(color, atual.Posicao, new Rectangle((int)atual.Posicao.X, (int)atual.Posicao.Y, (int)distancia, 1), rota.Viajante.Cor, angulo, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
                    }

                    //spriteBatch.DrawString(SpriteF)
                }
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
