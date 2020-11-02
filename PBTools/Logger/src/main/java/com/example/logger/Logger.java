package com.example.logger;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.ContentProvider;
import android.content.Context;
import android.content.DialogInterface;
import android.os.Environment;
import android.util.Log;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.nio.file.Files;
import java.util.ArrayList;
import java.util.List;

public class Logger
{
    private static final Logger instance = new Logger();
    private static final String LOGTAG = "AndroidLogger";
    private static String LOGPATH = "log.f";
    private static List<String> LogFakeFile;

    public  static Logger getInstance() { return instance; }

    public static Activity mainActivity;

    public interface AlertViewCallback
    {
        public void onButtonTapped(int id);
    }

    private long startTime;

    private Logger()
    {
        LogFakeFile = new ArrayList<String>();
        Log.i(LOGTAG, "Unity Android Logger");
        LOGPATH = Environment.getDataDirectory() + "/log.f";
        Log.i(LOGTAG, LOGPATH);
        startTime = System.currentTimeMillis();
    }

    public double getElapsedTime()
    {
        return (System.currentTimeMillis() - startTime) / 1000.0f;
    }

    public void writeLog(String string)
    {
        try
        {

            Log.i(LOGTAG, "Logueando: " + string);

            // Como no anda el archivo, uso esto (temporal)
            LogFakeFile.add(string);

            //PrintWriter out = new PrintWriter(new BufferedWriter(new FileWriter(LOGPATH,true)));

            //out.println(string);
            //out.close();
        }
        catch (Exception e)
        {
            Log.e(LOGTAG, e.getLocalizedMessage());
        }
    }

    public String[] getAllLogs()
    {
        try
        {
            return LogFakeFile.toArray(new String[]{});
            //BufferedReader reader = new BufferedReader(new FileReader(LOGPATH));
            //List<String> lines = new ArrayList<String>();

            //String s;
            //while((s=reader.readLine())!=null) {
            //    lines.add(s);
            //    System.out.println(s);
            //}
            //reader.close();

            // Como no anda el archivo, uso esto (temporal)
        }
        catch(Exception e)
        {
            Log.e(LOGTAG, e.getLocalizedMessage());
        }

        return new String[]{};
    }

    public void deleteLogs()
    {
        try
        {
            LogFakeFile.clear();
            //new FileWriter(LOGPATH, false).close();
        }
        catch(Exception e)
        {
            Log.e(LOGTAG, e.getLocalizedMessage());
        }
    }

    public void showAlertView(String[] strings, final AlertViewCallback callback)
    {
        if(strings.length < 3)
        {
            Log.i(LOGTAG,"Error - expected at least 3 strings, got: " + strings.length);
            return;
        }

        DialogInterface.OnClickListener myClickListener = (dialog, which) -> {
            dialog.dismiss();
            Log.i(LOGTAG, "Tapped: " + which);
            callback.onButtonTapped(which);
        };

        AlertDialog alertDialog = new AlertDialog.Builder(mainActivity)
                .setTitle(strings[0])
                .setMessage(strings[1])
                .setCancelable(false)
                .create();
        alertDialog.setButton(AlertDialog.BUTTON_NEUTRAL, strings[2], myClickListener);
        if(strings.length > 3)
            alertDialog.setButton(AlertDialog.BUTTON_NEGATIVE, strings[3], myClickListener);
        if(strings.length > 4)
            alertDialog.setButton(AlertDialog.BUTTON_POSITIVE, strings[4], myClickListener);
        alertDialog.show();
    }
}