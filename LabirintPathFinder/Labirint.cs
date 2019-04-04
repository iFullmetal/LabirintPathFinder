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
        int voidCount = 0;
        public Labirint()
        {
            //заполняю лабиринт значениями
            labirint = new Element[,]
               { { new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall()},

                { new Wall(), new Void(), new Void(), new Void(), new Void(), new Void(), new Void(), new Void(), new Void(), new Wall()},

                { new Wall(), new Void(), new Wall(), new Wall(), new Wall(), new Wall(), new Void(), new Wall(), new Wall(), new Wall()},

                { new Wall(), new Void(), new Void(), new Void(), new Void(), new Wall(), new Void(), new Wall(), new Void(), new Wall()},

                { new Wall(), new Void(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Void(), new Wall()},

                { new Wall(), new Void(), new Void(), new Void(), new Void(), new Void(), new Void(), new Wall(), new Void(), new Wall()},

                { new Wall(), new Void(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Void(), new Wall()},

                { new Wall(), new Void(), new Void(), new Void(), new Void(), new Void(), new Void(), new Wall(), new Void(), new Wall()},

                { new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Void(), new Wall()},

                { new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall(), new Wall()}
            };
            for (int i = 0; i < SIZE_Y; i++) //проставляю координаты для каждого элемента лабиринта(поленился сделать это в конструкторах при инициализации)
            {
                for (int j = 0; j < SIZE_X; j++)
                {
                    if (labirint[i, j] is Void) voidCount++;
                    labirint[i, j].X = j;
                    labirint[i, j].Y = i;
                }
            }
        }
        public bool canIGoThere(Element el) //проверка того, могу ли я встать в этот элемент. (он должен быть пустотой и непройденным). функция подразумевает, что этот элемент прилегает к "боту"
        {
            return el is Void && (el as Void).passed == false;
        }
        public int countWays(ref Coord coord) //метод подсчета путей и в тоже время смены текущей координаты на новую, если туда можно встать
        {
            int ways = 0;
            Coord result = coord;//изменяю не саму координату, а другую переменную, потому что оригинальная координата нужна в if'ах
            if (!isWall(coord.x + 1, coord.y) && canIGoThere(labirint[coord.y, coord.x + 1])) //проверка на то, что блок не стена и на то, что в него можно встать(он непройденный ранее)
            {
                ways++; //подсчитываю количество направлений, в которые можно пойти
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
            coord = result; //все проверки пройденны и теперь уже можно наконец стать в свободную позицию, если таковая найдена
            return ways;
        }
        public bool isWall(Coord coord) //проверка на то, лежит ли потакой координате стена и входит ли эта координата в лабиринт
        {
            if (coord.x >= SIZE_X || coord.y >= SIZE_Y || coord.x < 0 || coord.y < 0) return true; //проверка на то, что координата входит в лабирин
            else if (labirint[coord.y, coord.x] is Wall) return true; //проверка на стену. возвращается истинна
            else return false;
        }
        public bool isWall(int x, int y) //перегрузка для интов
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
            (labirint[pos.y, pos.x] as Void).passed = true; //текущая позиция становится пройденной

            pos.draw();
            Checkpoint currentCheckpoint = new Checkpoint(pos);
            int steps = 0;
            if (!isWall(start) && !isWall(finish)) //если обе координаты являются проходами и входят в лабиринт
            {
                int ways = 0; //количество направлений, в которые можно пойти
                do
                {
                    prevPos = pos; //запоминаю прошлую позицию
                    ways = countWays(ref pos); //рассчитываю количество возможных направлений + иду сразу же иду в какое-то из них, если таковое найдено
                    (labirint[pos.y, pos.x] as Void).passed = true;  //текущая позиция становится пройденной. на нее уже нельзя будет походить
                    if (ways > 1) // если путей больше одного, ставлю котрольную точку
                    {
                        Checkpoint temp = currentCheckpoint;
                        currentCheckpoint = new Checkpoint(prevPos);
                        currentCheckpoint.prev = temp; //каждая контрольная точка(кроме первой), знает прошлую контрольную точку. Такой вот список получился.
                     
                    }
                    currentCheckpoint.way.push(pos);  //отправляю текущую позицию в пройденный путь контрольной точки. У каждой контрольной точки свой путь, если
                    //сложить их пути в финале получится общий
                    if (ways == 0) //возвращаюсь к текущему чекпоинту, потому что зашел в тупик
                    {
                        pos.x = currentCheckpoint.X;
                        pos.y = currentCheckpoint.Y;
                        currentCheckpoint = currentCheckpoint.prev; //откатываю текущую контрольную точку к прошлой
                    }
                    if(pos == finish) //а вот и финиш
                    {
                        break;
                    }
                } while (currentCheckpoint != null); //цикл будет работать до тех пор, пока мы не дойдем до prev самого первого чекпоинта
                //поскольку самый первый чекпоинт был поставлен в координате start, то
                //это будет означать, что идти больше совсем некуда и попасть в finish невозможно

                bool isLabirintPassed = currentCheckpoint != null; //сохраняю результат на случай, если выход все же был найден, но в следующем цикле результат будет испорчен и if не отработает

                while (currentCheckpoint != null) //прохожусь по "списку" из контрольных точек, только вот если выход не найден, то currentCheckpoint уже изначально равен null и проходить будет не по чему
                {
                    currentCheckpoint.way.draw(); //вывожу путь каждого чекпоинта
                    steps += currentCheckpoint.way.getSteps(); //сохраняю количество шагов подпутя каждого чекпоинита
                    currentCheckpoint = currentCheckpoint.prev; //иду дальше
                }
                if(isLabirintPassed) //если лабиринт пройден, то возвращаю правду + вывожу информацию о пути
                {
                    Console.SetCursorPosition(0, 11);
                    Console.WriteLine("finish x = {0}, y = {1}", finish.x, finish.y);
                    Console.WriteLine("steps: {0}", steps);
                    return true;
                }
            }
            return false; //если в прошлый if не зашло, то лабиринт не пройден
        }
        public void draw()//отрисовка лабиринта
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
