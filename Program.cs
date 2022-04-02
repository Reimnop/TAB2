using TAB2;

using Tab2Main tab2Main = new Tab2Main();
tab2Main.Run(File.ReadAllText("token.txt")).GetAwaiter().GetResult();