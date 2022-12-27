namespace sun.reflect
{

    public interface FieldAccessor
    {

        object get(object v);

        bool getBoolean(object v);

        sbyte getByte(object v);

        ushort getChar(object v);

        double getDouble(object v);

        float getFloat(object v);

        int getInt(object v);

        long getLong(object v);

        short getShort(object v);

        void set(object v1, object v2);

        void setBoolean(object v1, bool v2);

        void setByte(object v, byte val);

        void setChar(object v, char val);

        void setDouble(object v, double val);

        void setFloat(object v, float val);

        void setInt(object v, int val);

        void setLong(object v, long val);

        void setShort(object v, short val);

    }

}
