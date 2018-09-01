public class Delivery {

    private Wire wire = null;
    public Machine to = null;
    public Machine from = null;
    public float delay_time = 0;
    public int bits = 0;

    public bool message_delivered = false;

    public Delivery(Machine to, Machine from, int bits, float delay, Wire wire)
    {
        this.to = to;
        this.from = from;
        this.bits = bits;
        delay_time = delay;
        this.wire = wire;
    }

    public void Update(float dt)
    {
        if (!message_delivered)
        {
            delay_time -= dt;
            if (delay_time <= 0)
            {
                DelivereBits();
            }
        }
    }

    private void DelivereBits()
    {
        to.ReceiveBits(bits, from);
        message_delivered = true;
    }
}