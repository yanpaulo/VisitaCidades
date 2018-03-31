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
        private Problema problema = new Problema();
        private Solucao solucao;
        private Texture2D bg;
        private Texture2D color;
        private GeneticAlgorithm ga;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;

            solucao = problema.SolucaoAleatoria();
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

            bg = new Texture2D(GraphicsDevice, problema.Mapa.Tamanho.Width, problema.Mapa.Tamanho.Height);
            color = new Texture2D(GraphicsDevice, 10, 10);

            bg.SetData(Enumerable.Range(0, bg.Width * bg.Height).Select(n => Color.White).ToArray());
            color.SetData(Enumerable.Range(0, color.Width * color.Height).Select(n => Color.White).ToArray());

            //População de no mmínimo 50 e máximo 70 indivíduos
            var population = new Population(50, 70, new CromossomoViajante(problema.Mapa.Locais.Count));
            var fitness = new FitnessViajante(problema);

            ga = new GeneticAlgorithm(population, fitness, new EliteSelection(), new OrderedCrossover(), new ReverseSequenceMutation())
            {
                CrossoverProbability = 0.75f,
                MutationProbability = 0.10f,
                Termination = new FitnessStagnationTermination(),
                //Executa com paralelismo
                TaskExecutor = new GeneticSharp.Infrastructure.Threading.SmartThreadPoolTaskExecutor()
            };

            //Evento.
            //O código dentro do bloco delegate{} é executado sempre que uma nova geração é criada pelo algoritmo.
            ga.GenerationRan += delegate
            {
                var chromosome = ga.BestChromosome as CromossomoViajante;
                //Converte o cromossomo em Solucao.
                solucao = problema.Solucao(chromosome.GetGenes().Select(g => (int)g.Value).ToList());
            };

            //Inicia o algoritmo.
            //Task.Run() faz com que a chamada a Start execute de forma assíncrona, sem bloquear a execução.
            Task.Run(() =>
            {
                ga.Start();
            });
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
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            
            spriteBatch.Draw(bg, problema.Mapa.Tamanho, Color.White);


            foreach (var local in problema.Mapa.Locais)
            {
                spriteBatch.Draw(color, local.Posicao, Color.Blue);
            }

            if (solucao != null)
            {
                foreach (var rota in solucao.Rotas)
                {
                    for (int i = 0; i < rota.Locais.Count - 1; i++)
                    {
                        var atual = rota.Locais[i];
                        var proximo = rota.Locais[i + 1];

                        var direcao = proximo.Posicao - atual.Posicao;
                        var distancia = Vector2.Distance(atual.Posicao, proximo.Posicao);
                        var angulo = Math.Atan2(direcao.Y, direcao.X);

                        spriteBatch.Draw(color, atual.Posicao, new Rectangle((int)atual.Posicao.X, (int)atual.Posicao.Y, (int)distancia, 1), rota.Viajante.Cor, (float)angulo, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
                    }
                } 
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
