package bulletin_publish.com;

import java.io.IOException;
import java.io.InputStream;

import org.apache.http.HttpEntity;
import org.apache.http.HttpHost;
import org.apache.http.HttpResponse;
import org.apache.http.StatusLine;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpUriRequest;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EncodingUtils;

public class http {

    public static int get (){
    	
     	DefaultHttpClient client = new DefaultHttpClient();
        HttpUriRequest request = new HttpGet("http://kenfujioka.name:80/ClientBin/Bulletin.txt");
        HttpHost host = new HttpHost("kenfujioka.name", 80);
	    
		HttpResponse response = null;
		
		request.setHeader("Host", "kenfujioka.name");
		request.setHeader("Accept", "text/html");
		request.setHeader("From", "kenzo@pronunsia.info");
		request.setHeader("User-Agent", "Android-Http-Client");
		
		try {
			response = client.execute(host, request);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			
			return -1;
		}
		
		//client.close();
		
		StatusLine status = null;
		
		if(response != null)
		{
			status = response.getStatusLine();
		}					
		
		if(status != null)
		{
			int temp = status.getStatusCode();
			return temp;
		}
		else
		{
			return -2;
		}
    }
    
    public static String getFile (){
    	
    	String temp = "";
    	
    	DefaultHttpClient client = new DefaultHttpClient();
        HttpUriRequest request = new HttpGet("http://kenfujioka.name:80/ClientBin/Bulletin.txt");
        HttpHost host = new HttpHost("kenfujioka.name", 80);
	    
		HttpResponse response = null;
		
		request.setHeader("Host", "kenfujioka.name");
		request.setHeader("Accept", "*/*");
		request.setHeader("From", "kenzo@pronunsia.info");
		request.setHeader("User-Agent", "Android-Http-Client");
		
		try {
			response = client.execute(host, request);
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();			
		}
		
		StatusLine status = null;
		
		if(response != null)
		{
			status = response.getStatusLine();
		}					
		
		if(status != null && status.getStatusCode() == 200)
		{
			HttpEntity entity = response.getEntity();
			int length = (int)entity.getContentLength();
			
			InputStream stream = null;
			try {
				stream = entity.getContent();
			} catch (IllegalStateException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
			byte[] byteArray = null;
			try {
				byteArray = new byte[length];
			} catch (Exception e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
			for(int x = 0; (length -  (x * 8)) >= 8; x++)
			{
				try {
					stream.read(byteArray, x * 8, 8);
				} catch (Exception e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
			
			int temp2 = (int)((int)length / 8) * 8;
			int temp3 = length - ((int)((int)length / 8) * 8);
			
			try {
				stream.read(byteArray, temp2, temp3);
			} catch (IOException e2) {
				// TODO Auto-generated catch block
				e2.printStackTrace();
			}
			
			try {
				entity.consumeContent();
			} catch (IOException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
			}
			
			try {
				stream.close();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
			temp = EncodingUtils.getString(byteArray, "CESU-8");
			
			/*
			char[] charArray = new char[byteArray.length];
		
			for(int x = 0; x < byteArray.length; x++)
			{
				charArray[x] = (char)byteArray[x];
			}
			
			temp = String.copyValueOf(charArray);		*/	
		}
		
		return temp.toString();
    }
}
