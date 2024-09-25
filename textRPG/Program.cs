using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;

namespace textRPG
{
    public struct Character
    {
        public string Name { get; }
        public int Level { get; set; }
        public int Attack { get; set; }
        public int Deffence { get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }
        public int DunClear { get; set; }
        public Character(string name, int level, int attack, int deffence, int health, int gold)
        {
            Name = name;
            Level = level;
            Attack = attack;
            Deffence = deffence;
            Health = health;
            Gold = gold;
            DunClear = 0; //초기값0
        }

    }

    public struct Item
    {
        public string Name { get; }
        public string Information { get; }
        public int Type { get; } //0.무기 1.방어구 2.잡화
        public int Attack { get; }
        public int Deffence { get; }
        public int Gold { get; }
        public bool IsEquippable { get; set; } //장착 아이템 구별
        public bool IsStoreItem { get; } //상점 아이템 여부 구별
        public bool IsPurchased { get; set; } //구매상태 구별

        public Item(string name, string information, int type, int attack, int deffence, int gold,bool isStoreItem, bool isEquippable = true)
        {
            Name = name;
            Information = information;
            Type = type;
            Attack = attack;
            Deffence = deffence;
            Gold = gold;
            IsEquippable = isEquippable;
            IsStoreItem = isStoreItem;
            IsPurchased = false; // 초기값설정
        }
    }
    internal class Program
    {
        static Character player;
        static List<Item> storeItems = new List<Item>(); //상점아이템저장소
        static List<Item> inventoryItems = new List<Item>(); //인벤토리아이템저장소

        
        //입력값체크
        static int CheckInput(int min, int max)
        {
            int choice;

            while (true)
            {
                Console.WriteLine("\n\n원하시는 행동을 입력해주세요.");
                Console.Write("\n\n>>>  ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                else
                {
                    Console.WriteLine($"\n\n잘못된 입력! 다시 시도하세요.");
                }

            }
                


        }
        //메인
        static void Main(string[] args)
        {
            //콘솔 창 높이 설정
            Console.WindowHeight = 40;
            //초기플레이어
            player = new Character("Unity6기생_김아무개", 1, 10, 5, 100, 1500);
            
            //상점아이템셋
            storeItems.Add(new Item("나무검", "나무로된 검", 0, 5, 0, 500, true));
            storeItems.Add(new Item("철검", "철로된 검", 0, 10, 0, 500, true));
            storeItems.Add(new Item("천옷", "천으로된 옷", 1, 0, 5, 500, true));
            storeItems.Add(new Item("철갑옷", "철로된 검", 1, 0, 10, 500, true));
            storeItems.Add(new Item("쓸모없는 책", "무슨 책인지 모르겠다", 2, 0, 0, 100, true, false));


            StartMenu();
        }

        private static void StartMenu()
          
        {
            Console.Clear();
            Console.WriteLine("※※Welcom to 스파르타 던☆전에 오신 너 환?영!!★★");
            Console.WriteLine("\n\n§§당신 이 곳에서 가능 하다 행동결정 ☜☜");
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("\n\n\t1. 상태보기(상태창!!이라고 외치면서 번호입력요망) ");
            Console.WriteLine("\t2. 인벤토리");
            Console.WriteLine("\t3. 상점입장");
            Console.WriteLine("\t4. 던전입장");
            Console.WriteLine("\t5. 휴식하기");
            Console.WriteLine("\t6. 게임종료");
            
            Console.WriteLine("\n\n==============================================================");

            int choice = CheckInput(1, 7);

            switch (choice)
            {
                case 1:
                    StatusMenu();
                    break;
                case 2:
                    InventoryMenu();
                    break;
                case 3:
                    StoreMenu();
                    break;
                case 4:
                    DungeonMenu();
                    break;
                case 5:
                    RestMenu();
                    break;
                case 6:
                    ExitMenu();
                    break;

            }
        }
        private static void StatusMenu()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n이곳에서 확인한다 너 현재상태");
            Console.WriteLine("\n\n============================================");
            Console.WriteLine("\n\nLEVEL: " + player.Level.ToString("00"));
            Console.WriteLine(player.Name);

            int bonusAtk = EquipAtk();
            int bonusDef = EquipDef();

            Console.WriteLine("공격력 \t: " + (player.Attack+bonusAtk));
            Console.WriteLine("방어력 \t: " + (player.Deffence+bonusDef));
            Console.WriteLine("체력 \t: " + player.Health);
            Console.WriteLine("GOLD \t: " + player.Gold);
            Console.WriteLine("\n0. 나가기");
            Console.WriteLine("\n\n============================================");

            int choice = CheckInput(0, 0);

            switch (choice)
            {
                case 0:
                    StartMenu();
                    break;
            }
        }

        private static void InventoryMenu()
        {
            Console.Clear();
            Console.WriteLine("\n\n===============인벤토리========================");

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                string equipMark = ""; //장착여부마커

                //현재아이템이 장착된 장비와 동일한지 확인
                if (inventoryItems[i].Equals(equippedWeapon))
                {
                    equipMark = "[E] ";
                }
                else if (inventoryItems[i].Equals(equippedArmor))
                {
                    equipMark = "[E] ";
                }
                //아이템정보출력
                Console.WriteLine($"{i + 1}. {equipMark}{inventoryItems[i].Name} | 공격력: {inventoryItems[i].Attack} | 방어력: {inventoryItems[i].Deffence} | {inventoryItems[i].Information} | {inventoryItems[i].Gold}G");

            }
            Console.WriteLine("\n0. 나가기");
            Console.WriteLine("\n\n================================================");
            Console.WriteLine("\n\n번호를 입력하면 해당 장비를 장착합니다.");

            int choice = CheckInput(0, inventoryItems.Count);

            if (choice == 0)
            {
                StartMenu();
            }
            else
            {
                EquipItem(choice - 1); //선택아이템장착
            }

        }
        // 장착선언초기화
        private static Item? equippedWeapon = null;
        private static Item? equippedArmor = null;

        private static void EquipItem(int index)
        {
            Item item = inventoryItems[index];

            if (!item.IsEquippable) 
            {
                Console.WriteLine("\n\n이 아이템은 장착할 수 없습니다.");
                return;
            }

            //기존 장착된 아이템 해제
            if (item.Type == 0)
            {
                if (equippedWeapon.HasValue) //무기장착
                {
                    Console.WriteLine($"\n\n{equippedWeapon.Value.Name}이(가) 해제되었습니다.");
                    equippedWeapon = null;
                }
                equippedWeapon = item; 
            }
            else if (item.Type == 1) //방어구장착
            {
                if (equippedArmor.HasValue)
                {
                    Console.WriteLine($"\n\n{equippedArmor.Value.Name}이(가) 해제되었습니다.");
                    equippedArmor = null;
                }
                equippedArmor = item; 
            }

            Console.WriteLine($"\n\n{item.Name}을(를) 장착했씁니다.");
            Console.WriteLine("\n\n0. 나가기");
            CheckInput(0, 0);
            InventoryMenu();
        }
        //장착된 아이템의 효과 반영
        private static int EquipAtk() => equippedWeapon?.Attack ?? 0; // 장착된 무기 공격력 리턴, 없으면0
        private static int EquipDef() => equippedArmor?.Deffence ?? 0; // 장착된 옷 방어력 리턴, 없으면0
        //상점메뉴
        private static void StoreMenu()
        {
            Console.Clear();
            Console.WriteLine("\n\n===============상점========================");

            Console.WriteLine("\n\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");


            Console.WriteLine("\n\n================================================");

            int choice = CheckInput(0, 2);

            switch (choice)
            {
                case 1:
                    BuyItemMenu();
                    break;
                case 2:
                    SellItemMenu();
                    break;
                case 0:
                    StartMenu();
                    break;
            }
        }

        //구매 메서드
        private static void BuyItem(int index)
        {
            Item item = storeItems[index];
            int choice;

            if (item.IsPurchased) //구매한 아이템인지 확인
            {
                Console.WriteLine("\n\n이미 구매한 아이템입니다.");
                Console.WriteLine("\n\n0. 나가기");
                choice = CheckInput(0, 0);
                if(choice == 0)
                {
                    BuyItemMenu(); //구매페이지로되돌아가기
                }
                return;
            }

            if (player.Gold >= item.Gold) //구매가능조건
            {
                player.Gold -= item.Gold; //골드차감
                inventoryItems.Add(item); //인벤토리에 아이템추가
                item.IsPurchased = true; //아이템구매완료상태로 변경
                storeItems[index] = item; //상점리스트업데이트
                Console.WriteLine($"\n\n{item.Name}을(를) 구매했습니다.");
            }
            else
            {
                Console.WriteLine("\n\n골드가 부족합니다");
            }

            Console.WriteLine("\n0. 나가기");
            choice = CheckInput(0, 0);

            switch (choice)
            {
                case 0:
                    StartMenu();
                    break;
            }
        }
        //구매메뉴
        private static void BuyItemMenu()

        {
            Console.Clear();
            Console.WriteLine("\n\n===============아이템 구매========================");
            Console.WriteLine($"\n\n보유 골드 : {player.Gold}G");
            //상점아이템 출력
            for (int i = 0; i < storeItems.Count; i++)
            {
                Item item = storeItems[i];
                string purchaseMark = storeItems[i].IsPurchased ? "(구매완료)" : ""; //구매상태표시
                Console.WriteLine($"{i + 1}. {storeItems[i].Name}\t | {storeItems[i].Information} | 공격력 : {storeItems[i].Attack} | 방어력 : {storeItems[i].Deffence} | 가격 : {storeItems[i].Gold}G | {purchaseMark}");
            }

            Console.WriteLine("0. 구매 취소 (돌아가기)");

            int choice = CheckInput(0, storeItems.Count);

            if(choice == 0)
            {
                StoreMenu();
            }
            else
            {
                BuyItem(choice - 1);
            }
        }

        //판매메서드
        private static void SellItem(int index)
        {
            Item item = inventoryItems[index];
            int sellPrice = (int)Math.Floor(item.Gold * 0.85f);
            player.Gold += sellPrice;
            inventoryItems.RemoveAt(index);

            Console.WriteLine($"\n\n{item.Name}을(를) {sellPrice}에 판매했습니다");
            Console.WriteLine("\n0. 나가기");

            int choice = CheckInput(0, 0);

            switch (choice)
            {
                case 0:
                    StartMenu();
                    break;
            }
        }
        //판매메뉴
        private static void SellItemMenu()
        {
            Console.Clear();
            Console.WriteLine("\n\n===============아이템 판매========================");
            Console.WriteLine($"\n\n보유 골드 : {player.Gold}G");
            
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                int sellPrice = (int)(inventoryItems[i].Gold * 0.85f);
                Console.WriteLine($"{i + 1}. {inventoryItems[i].Name} | {inventoryItems[i].Information} | 공격력: {inventoryItems[i].Attack} | 방어력: {inventoryItems[i].Deffence} | 판매가: {sellPrice}G");
            }

            Console.WriteLine("\n0. 나가기");

            int choice = CheckInput(0, inventoryItems.Count);

            if (choice == 0)
            {
                StartMenu();
            }
            else if (choice > 0 && choice <= inventoryItems.Count)
            {
                SellItem(choice - 1);
            }
        }
        //던전메뉴
        private static void DungeonMenu()
        {
            Console.Clear();
            Console.WriteLine("\n\n===============던전========================");
            Console.WriteLine("\n1. 쉬운 던전 (권장 방어력: 5)");
            Console.WriteLine("2. 일반 던전 (권장 방어력: 10)");
            Console.WriteLine("3. 어려운 던전 (권장 방어력: 15)");
            Console.WriteLine("0. 나가기");

            int choice = CheckInput(0, 3);

            switch (choice)
            {
                case 1:
                    EnterDungeon(5, 1000);
                    break;
                case 2:
                    EnterDungeon(10, 1700);
                    break;
                case 3:
                    EnterDungeon(15, 2500);
                    break;
                case 0:
                    StartMenu();
                    break;
            }
        }

        private static Random random = new Random();
        //던전 메서드
        private static void EnterDungeon(int recommendDef, int baseGoldReward)
        {
            int choice;
            Console.Clear();
            Console.WriteLine("\n\n던전던전던전진입진입진입");

            if (player.Deffence < recommendDef)
            {
                if (random.NextDouble() < 0.4) //40%확률로 던전 실패 
                {
                    Console.WriteLine("공략실패~ㅋㅋ, 체력 반깎였지롱");
                    player.Health /= 2;
                    Console.WriteLine($"\n\n현재 체력: {player.Health}");
                    
                    Console.WriteLine("\n\n0. 나가기");

                    choice = CheckInput(0, 0);
                    
                    switch (choice)
                    {
                        case 0:
                            StartMenu();
                            break;
                    }
                }
            }

            //던전클리어
            Console.WriteLine("경)공략 성공!!!(축");

            player.DunClear++;
            LevelUp();

            int defenseGap = Math.Abs(player.Deffence - recommendDef); //절대값으로처리

            int adjustFactor = player.Deffence < recommendDef ? defenseGap : -defenseGap; //이제 음수표현 가능

            int minHpLoss = Math.Max(20 + adjustFactor, 0);
            int maxHpLoss = Math.Max(35 + adjustFactor, 0);
            int totalLoss = random.Next(minHpLoss, maxHpLoss + 1);

            player.Health -= totalLoss;

            if (player.Health < 0) player.Health = 0;

            Console.WriteLine($"\n\n남은 체력 : {player.Health}");

            //골드보상
            int addiGoldPercent = random.Next(player.Attack, player.Attack * 2 + 1);
            int additionalGold = baseGoldReward * addiGoldPercent / 100;
            int totalReward = baseGoldReward + additionalGold;

            player.Gold += totalReward;
            Console.WriteLine($"골드 보상 \t: {totalReward}G");
            Console.WriteLine($"현재 보유 골드 \t: {player.Gold}");

            Console.WriteLine("\n\n0. 나가기");

            choice = CheckInput(0, 0);

            switch (choice)
            {
                case 0:
                    StartMenu();
                    break;

            }
        }
        //레벨업메서드
        private static void LevelUp()
        {
            int requiredClears = 0;

            switch (player.Level)
            {
                case 1: requiredClears = 1; break;
                case 2: requiredClears = 2; break;
                case 3: requiredClears = 3; break;
                case 4: requiredClears = 4; break;
                default: return;
            }

            if (player.DunClear >= requiredClears)
            {
                player.Level++;
                player.Attack += 1;
                player.Deffence += 1;
                Console.WriteLine($"\n\n레벨업했습니다. 공격력과 방어력이 상승합니다. 현재 레벨: {player.Level}");
            }
        }
        //휴식메뉴
        private static void RestMenu() 
        {
            Console.Clear();
            Console.WriteLine("\n\n===============여관========================");
            Console.WriteLine("\n\n500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {0})", player.Gold);
            Console.WriteLine("\n\n1. 휴식하기");
            Console.WriteLine("0. 나가기");

            int choice = CheckInput(0, 1);
            switch (choice) 
            {
                case 1: Rest();
                     break;
                case 0: StartMenu();
                    break;
            }

        }
        //휴식메서드
        private static void Rest()
        {
            const int restCost = 500;
            const int maxHealth = 100;

            if (player.Gold >= restCost)
            {
                player.Gold -= restCost;
                player.Health = maxHealth;
                Console.WriteLine("\n\n휴식을 완료했습니다");
            }
            else
            {
                Console.WriteLine("\n\n골드가 부족합니다.");
            }

            Console.WriteLine("\n0. 나가기");
            int choice = CheckInput(0, 0);
            switch (choice)
            {
             
                case 0:
                    StartMenu();
                    break;
            }

        }


        private static void ExitMenu() 
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\t\t바이바이");
            Environment.Exit(0);
        }


    }
}
