public class AnimationFrame
{
    public Frame frame;
    public string id;
    public string offset;
    public bool rotated;
    public string sourceColorRect;
    public string sourceSize;

    public class Frame
    {
        public int h;
        public int w;
        public int x;
        public int y;
    }
}