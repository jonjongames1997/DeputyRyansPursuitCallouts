namespace DeputyRyansPursuitCallouts.DeputyRyansStuff
{
    internal static class Helper
    {
        internal static Ped MainPlayer => Game.LocalPlayer.Character;
        internal static Random Rndm = new(DateTime.Now.Millisecond);
    }
}