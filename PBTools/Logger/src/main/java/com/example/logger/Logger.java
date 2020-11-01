package com.example.logger;
import android.util.Log;

public class Logger
{
    private static final Logger instance = new Logger();
    private static final String LOGTAG = "AndroidLogger";

    public  static Logger getInstance() { return instance; }

    private long startTime;

    private Logger()
    {
        Log.i(LOGTAG, "Unity Android Logger");
        startTime = System.currentTimeMillis();
    }

    public double getElapsedTime()
    {
        return (System.currentTimeMillis() - startTime) / 1000.0f;
    }
}