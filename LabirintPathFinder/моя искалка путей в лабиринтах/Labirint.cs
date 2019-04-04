using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabirintPathFinder
{
    class Labirint
    {
        Element[,] labirint;
        const int SIZE_X = 10;
        const int SIZE_Y = 10;
        public Labirint()
        {
            labirint = new Element[,]
               { { new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall()},

                { new Wall(), new Void(), new Void(), new Void(), new Void(), new Void(), new Void(), new Void(), new Void(), new Wall()},

                { new Wall(), new Void(), new Wall(), new Wall(), new Wall(), new Wall(), new Void(), new Void(), new Void(), new Wall()},

                { new Wall(), new Void(), new Void(), new Void(), new Void(), new Wall(), new Void(), new Void(), new Void(), new Wall()},

                { new Wall(), new Void(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Void(), new Wall()},

                { new Wall(), new Void(), new Void(), new Void(), new Void(), new Void(), new Void(), new Wall(), new Void(), new Wall()},

                { new Wall(), new Void(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Void(), new Wall()},

                { new Wall(), new Void(), new Void(), new Void(), new Void(), new Void(), new Void(), new Wall(), new Void(), new Wall()},

                { new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Void(), new Wall()},

                { new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall()}
            };
            for (int i = 0; i < SIZE_Y; i++)
            {
                for (int j = 0; j < SIZE_X; j++)
                {
                    labirint[i, j].X = j;
                    labirint[i, j].Y = i;
                }
            }
        }
        public bool canIGoThere(Element el)
        {
            return el is Void && (el as Void).passed == false;
        }
        public int countWays(ref Coord coord)
        {
            int ways = 0;
            Coord result = coord;
            if (!isWall(coord.x + 1, coord.y) && canIGoThere(labirint[coord.y, coord.x + 1]))
            {
                ways++;
                result = new Coord(coord.x + 1, coord.y);
            }
            if (!isWall(coord.x - 1, coord.y) && canIGoThere(labirint[coord.y, coord.x - 1]))
            {
                ways++;
                result = new Coord(coord.x - 1, coord.y);
            }
            if (!isWall(coord.x, coord.y - 1) && canIGoThere(labirint[coord.y - 1, coord.x]))
            {
                ways++;
                result = new Coord(coord.x, coord.y - 1);
            }
            if (!isWall(coord.x, coord.y + 1) && canIGoThere(labirint[coord.y + 1, coord.x]))
            {
                ways++;
                result = new Coord(coord.x, coord.y + 1);
            }
            coord = result;
            return ways;
        }
        public bool isWall(Coord coord) //проверка на то, лежит ли потакой координате стена и входит ли эта координата в лабиринт
        {
            if (coord.x >= SIZE_X || coord.y >= SIZE_Y || coord.x < 0 || coord.y < 0) return true; //проверка на то, что координата входит в лабирин
            else if (labirint[coord.y, coord.x] is Wall) return true; //проверка на стену. возвращается истинна
            else return false;
        }
        public bool isWall(int x, int y)
        {
            Coord coord = new Coord(x, y);
            if (coord.x >= SIZE_X || coord.y >= SIZE_Y || coord.x < 0 || coord.y < 0) return true;
            else if (labirint[coord.y, coord.x] is Wall) return true;
            else return false;
        }
        public bool isPassable(Coord start, Coord finish)
        {
            Coord pos = start;
            Coord prevPos;//прошлая позиция. необходима для создания чекпоинта уже после хода, когда становится известным количество ходов, но pos уже изменен в функции countWays
            (labirint[pos.y, pos.x] as Void).passed = true;

            pos.draw();
            Checkpoint currentCheckpoint = new Checkpoint(pos);
            int steps = 0;
            if (!isWall(start) && !isWall(finish)) //если обе координаты являются проходами и входят в лабиринт
            {
                int ways = 0;
                do
                {
                    prevPos = pos;
                    ways = countWays(ref pos);
                    (labirint[pos.y, pos.x] as Void).passed = true;
                    if (ways > 1) // если путей больше еденицы, ставлю котрольную точку
                    {
                        Checkpoint temp = currentCheckpoint;
                        currentCheckpoint = new Checkpoint(prevPos);
                        currentCheckpoint.prev = temp; //каждая контрольная точка(кроме первой), знает прошлую контрольную точку. Такой вот список получился.
                     
                    }
                    if(currentCheckpoint != null) currentCheckpoint.way.push(pos);
                    if (ways == 0) //возвращаюсь к прошлому чекпоинту
                    {
                        pos.x = currentCheckpoint.X;
                        pos.y = currentCheckpoint.Y;
                        currentCheckpoint = currentCheckpoint.prev;
                    }
                    if(pos == finish)
                    {
                        break;
                    }
                    //pos.draw();
                    steps++;
                } while (true);
                
                Console.SetCursorPosition(0, 11);
                Console.WriteLine("finish x = {0}, y = {1}", finish.x, finish.y);
                Console.WriteLine("ways detected: {0}\nsteps: {1}", ways, steps);
                while (currentCheckpoint != null)
                {
                    currentCheckpoint.way.draw();
                    //Console.WriteLine("checkpoint x = {0}, y = {1}", currentCheckpoint.X, currentCheckpoint.Y);
                    currentCheckpoint = currentCheckpoint.prev;
                }

            }
            return false;
        }
        public void draw()
        {
            for (int i = 0; i < SIZE_Y; i++)
            {
                for (int j = 0; j < SIZE_X; j++)
                {
                    labirint[i, j].draw();
                }
            }
        }
    }
}
