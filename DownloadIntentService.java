package bulletin_publish.com;

import android.app.IntentService;
import android.app.PendingIntent;
import android.app.PendingIntent.CanceledException;
import android.content.Intent;
import android.util.Log;

import java.io.InputStream;
import java.net.MalformedURLException;
import java.net.URL;

public class DownloadIntentService extends IntentService {

    private static final String TAG = DownloadIntentService.class.getSimpleName();
    public static final String PENDING_RESULT_EXTRA = "pending_result";
    public String file = "";
    
    public DownloadIntentService() {
        super(TAG);
    }

	@Override
	protected void onHandleIntent(Intent intent) {
		// TODO Auto-generated method stub
        PendingIntent reply = intent.getParcelableExtra(PENDING_RESULT_EXTRA);

        file = http.getFile();
        
        if(file == "" || file == null)
        {
        	file = " ";
        }
        
        Intent result = new Intent();
        result.putExtra("the_file", file.toCharArray());
        
        try {
			reply.send(this, 2, result);
		} catch (CanceledException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}
