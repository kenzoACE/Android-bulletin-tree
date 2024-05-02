package bulletin_publish.com;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.Dialog;
import android.app.PendingIntent;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.view.ContextMenu;
import android.view.ContextMenu.ContextMenuInfo;
import android.view.Gravity;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AbsListView;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.BaseExpandableListAdapter;
import android.widget.Button;
import android.widget.ExpandableListAdapter;
import android.widget.ExpandableListView;
import android.widget.ExpandableListView.ExpandableListContextMenuInfo;
import android.widget.ExpandableListView.OnChildClickListener;
import android.widget.GridView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;
import bulletin_publish.com.BulletinFile;
import bulletin_publish.com.http;
import android.speech.tts.TextToSpeech;
import android.speech.tts.TextToSpeech.OnInitListener;

public class Bulletin2Activity extends Activity {

	String file = "";
	ExpandableListAdapter mAdapter = null;
	BaseAdapter bAdapter = null; 
	BaseAdapter bAdapter2 = null; 
	int headingNum = 0;
	AlertDialog alert = null;
	String alertText = "";
	int[] expandedTag = new int[65];
	
	private TextToSpeech myTts;

	
	/** Called when the activity is first created. */
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.main);

		PendingIntent pendingResult = createPendingResult(1, new Intent(), 0);
		Intent intent = new Intent(getApplicationContext(), DownloadIntentService.class);

//		intent.putExtra(" ", " ");
		intent.putExtra(DownloadIntentService.PENDING_RESULT_EXTRA, pendingResult);
		startService(intent);
				
		OnInitListener ttsInitListener = new OnInitListener() {
			public void onInit(int version) {
				//do nothing
			}
		};

		myTts = new TextToSpeech(this, ttsInitListener);

		for(int x = 0; x < 65; x++)
		{
			expandedTag[x] = -1;
		}
		
		Toast.makeText(getBaseContext(), "Downloading data... please wait", Toast.LENGTH_LONG).show();
		
		if(true)
		{
			TextView text = (TextView) findViewById(R.id.editText1);
			text.setVisibility(8);
			final GridView gridView = (GridView) findViewById(R.id.gridView1);
			gridView.setVisibility(8);
			final Button button = (Button) findViewById(R.id.button1);
			button.setVisibility(0);
			final ExpandableListView list = (ExpandableListView) findViewById(R.id.expandableListView1);
			final ListView list2 = (ListView) findViewById(R.id.listView1);
			list2.setVisibility(8);
			final Button button2 = (Button) findViewById(R.id.button2);
			button2.setVisibility(8);

			button.setOnClickListener(new View.OnClickListener() {
				@Override
				public void onClick(View v) {
					mAdapter = new MyExpandableListAdapter();
					list.setAdapter(mAdapter);
					
					button.setVisibility(8);
					button2.setVisibility(8);
					gridView.setVisibility(8);
					list2.setVisibility(8);
					list.setVisibility(0);
				}
			});

			button2.setOnClickListener(new View.OnClickListener() {
				@Override
				public void onClick(View v) {
					if(list2.VISIBLE == list2.getVisibility())
					{
						list2.setVisibility(8);
						gridView.setVisibility(0);
					}
					else
					{
						gridView.setVisibility(8);
						list2.setAdapter(bAdapter2);
						list2.showContextMenu();
						list2.setVisibility(0);
					}
				}
			});

			// Set up our adapter
			mAdapter = new MyExpandableListAdapter();
			list.setAdapter(mAdapter);

			OnChildClickListener onClick = new OnChildClickListener()
			{
				@Override
				public boolean onChildClick(final ExpandableListView parent, View v,
						int groupPosition, int childPosition, long id) {

					list.setVisibility(8);

					Toast.makeText(getBaseContext(), "Opening tree...",
							Toast.LENGTH_SHORT).show();

					String heading = (String)(mAdapter.getGroup(groupPosition));

					try
					{
						headingNum = Integer.parseInt(heading.substring(0, heading.indexOf(":")).replace(" ", ""));
					}
					catch (Exception e)
					{
						char[] chars = new char[10];
						chars = heading.substring(3, heading.indexOf(":")).toCharArray();

						String temp2 = String.copyValueOf(chars, 1, chars.length - 1);

						temp2 = temp2.replaceFirst(" ", "");

						headingNum = Integer.parseInt(temp2);
					}
					GridView gridview = (GridView) findViewById(R.id.gridView1);
					bAdapter = new TextAdapter(getBaseContext());
					bAdapter2 = new TextAdapter2(getBaseContext());
					gridview.setAdapter(bAdapter);
					gridView.showContextMenu();

					gridView.setVisibility(0);
					button.setVisibility(0);
					button2.setVisibility(0);

					gridview.setOnItemClickListener(new OnItemClickListener() {
						@Override
						public void onItemClick(AdapterView<?> arg0, View arg1,
								int arg2, long arg3) {
							GridView gridview = (GridView) findViewById(R.id.gridView1);
							TextAdapter adapter = (TextAdapter) gridView.getAdapter();

							String text = (String) adapter.getItem(0);
							String childText = (String)adapter.getItem(arg2);

							if(childText == null)
							{
								return;
							}

							while(childText.startsWith("=>"))
							{
								childText = childText.replaceFirst("=>", "");
							}

							int heading = Integer.parseInt(text.substring(0, text.indexOf(":")));
							int childNum = -1;

							while (childNum == -1)
							{
								try
								{
									childNum = Integer.parseInt(childText.substring(0, childText.indexOf(":")));
								}
								catch (Exception e)
								{
									childText = childText.substring(1);
								}
							}

							if(arg2 == 0)
							{
								showDialog(heading);
							}
							else
							{
								showDialog(childNum + heading * 257);
							}
						}
					});

					list2.setOnItemClickListener(new OnItemClickListener() {

						@Override
						public void onItemClick(AdapterView<?> arg0, View arg1,
								int arg2, long arg3) {
														
							 if(arg2 == 0)
			 	               {
			 	                 String text2 = BulletinFile.GetHeading(headingNum, file);
			 	                 myTts.stop();
			 	                 myTts.speak(text2, myTts.QUEUE_FLUSH, null);
			 	                 return;
			 	               }
							
							ListView listview = (ListView) findViewById(R.id.listView1);
							TextAdapter2 adapter = (TextAdapter2) listview.getAdapter();

							String text = (String) adapter.getItem(0);
							String childText = (String)adapter.getItem(arg2);

							if(childText == null)
							{
								return;
							}
							
							if(childText.startsWith("+"))
							{
								childText = childText.substring(1);
							}
							else if(childText.startsWith("-"))
							{
								childText = childText.substring(1);
							}
							
							while(childText.startsWith("=>"))
							{
								childText = childText.replaceFirst("=>", "");
							}

							int heading = Integer.parseInt(text.substring(0, text.indexOf(":")));
							int childNum = -1;

							try
							{
								childNum = Integer.parseInt(childText.substring(0, childText.indexOf(":")));
							}
							catch (Exception e)
							{
								childText = childText.substring(1);
							}
							
							String text2 = "";

							if(arg2 == 0)
							{
								text2 = BulletinFile.GetHeading(headingNum, file);
							}
							else
							{
								text2 = BulletinFile.GetComment(headingNum, file, childNum);
							}

							myTts.stop();
							myTts.speak(text2, myTts.QUEUE_FLUSH, null);

							int number = -1;
							
							try
							{
								number = BulletinFile._GetComment(heading, file, Integer.parseInt(childText.substring(0 , childText.indexOf(":")))) + 2;
							}
							catch(Exception e)
							{
//								number = BulletinFile._GetComment(heading, file, Integer.parseInt(childText.substring(0 , childText.indexOf(":") - 1)));
							}
							
							if(expandedTag[number] == 0)
							{
								expandedTag[number] = 1;
							}
							else if(expandedTag[number] == 1)
							{
								expandedTag[number] = 0;
							}

							bAdapter2 = new TextAdapter2(getBaseContext());
							list2.setAdapter(bAdapter2);
						}
					});

					return true;
				}
			};

			list.setOnChildClickListener(onClick);

			boolean debug = list.showContextMenu();
		}
		else
		{
			TextView text = (TextView) findViewById(R.id.editText1);
			text.setText("No data.  Please connect to the internet.");
			text.setVisibility(0);
		}
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
	    
		try
		{
			file = String.copyValueOf(data.getCharArrayExtra("the_file"));
		}
		catch (Exception e)
		{
			e.getMessage();
			file = " ";
		}
		
		if(file.length() < 5)
		{
			TextView text = (TextView) findViewById(R.id.editText1);
			text.setText("No data.  Please connect to the internet.");
			text.setVisibility(0);
		}
		else
		{
			Toast.makeText(getBaseContext(), "Please press back button", Toast.LENGTH_LONG).show();
		}
				
	    super.onActivityResult(requestCode, resultCode, data);
	}
	
	@Override
	protected Dialog onCreateDialog(int id) {
		Dialog dialog = null;
		String text = "";

		if(id == headingNum)
		{
			text = BulletinFile.GetHeading(headingNum, file);
		}
		else
		{
			text = BulletinFile.GetComment(headingNum, file, id - headingNum * 257);
		}

		AlertDialog.Builder builder = new AlertDialog.Builder(this);
		builder.setMessage(text)
		.setCancelable(true)
		.setPositiveButton("Close", new DialogInterface.OnClickListener() {
			@Override
			public void onClick(DialogInterface dialog, int id) {
				dialog.cancel();
			}
		});
		dialog = builder.create();

		return dialog;
	}

	public boolean onChildClick (ExpandableListView parent, View v, int groupPosition, int childPosition, long id)
	{
		Toast.makeText(this,"Child " + childPosition + " clicked in group " + groupPosition,
				Toast.LENGTH_SHORT).show();

		return true;
	}

	@Override
	public void onCreateContextMenu(ContextMenu menu, View v, ContextMenuInfo menuInfo) {
		menu.setHeaderTitle("publish Bulletin");
		menu.add(0, 0, 0, R.string.hello);
	}

	@Override
	public boolean onContextItemSelected(MenuItem item) {
		ExpandableListContextMenuInfo info = (ExpandableListContextMenuInfo) item.getMenuInfo();

		String title = ((TextView) info.targetView).getText().toString();

		int type = ExpandableListView.getPackedPositionType(info.packedPosition);
		if (type == ExpandableListView.PACKED_POSITION_TYPE_CHILD) {
			int groupPos = ExpandableListView.getPackedPositionGroup(info.packedPosition); 
			int childPos = ExpandableListView.getPackedPositionChild(info.packedPosition); 
			Toast.makeText(this, title + ": Child " + childPos + " clicked in group " + groupPos,
					Toast.LENGTH_SHORT).show();
			return true;
		} else if (type == ExpandableListView.PACKED_POSITION_TYPE_GROUP) {
			int groupPos = ExpandableListView.getPackedPositionGroup(info.packedPosition); 
			Toast.makeText(this, title + ": Group " + groupPos + " clicked", Toast.LENGTH_SHORT).show();
			return true;
		}

		return false;
	}

	/**
	 * A simple adapter which maintains an ArrayList of photo resource Ids. 
	 * Each photo is displayed as an image. This adapter supports clearing the
	 * list of photos and adding a new photo.
	 *
	 */
	public class MyExpandableListAdapter extends BaseExpandableListAdapter {
		// Sample data set.  children[i] contains the children (String[]) for groups[i].

		public String[] groups ()
		{
			String temp = "";
			String file2 = file.toString();

			file2 = file2.replace("\0", "");
			file2 = file2.replace("\r\n", "");
			//file = file.replace(":@;@", ":@;@\r\n");

			int index2 = 0;
			String[] listString = new String[256];
			int index = 0;

			while (file2.length() > 5)
			{
				temp = file2.substring(0, file2.indexOf(";@") + 2);
				file2 = file2.replace(temp, "");

				index2 = temp.indexOf(":@");
				index2 = temp.indexOf(":@", index2 + 1);
				index2 = temp.indexOf(":@", index2 + 1);

				listString[index] = temp.substring(0, index2);

				index++;
			}

			String[] groupString = new String[index];

			for(int x = 0; x < index; x++)
			{
				groupString[x] = "   " + listString[x].substring(0, listString[x].length());
			}

			return groupString;
		}

		public String[][] children()
		{
			String temp = "";
			String file2 = file.toString();

			file2 = file2.replace("\0", "");
			file2 = file2.replace("\r\n", "");

			int index2 = 0;
			String[] listString = new String[256];
			int index = 0;
			int index3 = 0;

			while (file2.length() > 5)
			{
				temp = file2.substring(0, file2.indexOf(";@") + 1);
				file2 = file2.replace(temp, "");

				index2 = temp.indexOf(":@");
				index2 = temp.indexOf(":@", index2 + 1);
				index3 = index2;
				index2 = temp.indexOf(":@", index2 + 1);
				index2 = temp.indexOf(":@", index2 + 1);

				listString[index] = temp.substring(index3, index2);

				index++;
			}


			String[][] childString = new String[index][1];

			for(int x = 0; x < index; x++)
			{
				childString[x][0] = listString[x].substring(0, listString[x].length());
			}

			return childString;
		}

		private String[] groups = groups();
		private String[][] children = children();


		@Override
		public Object getChild(int groupPosition, int childPosition) {
			return children[groupPosition][childPosition];
		}

		@Override
		public long getChildId(int groupPosition, int childPosition) {
			return childPosition;
		}

		@Override
		public int getChildrenCount(int groupPosition) {
			return children[groupPosition].length;
		}

		public TextView getGenericView() {
			// Layout parameters for the ExpandableListView
			AbsListView.LayoutParams lp = new AbsListView.LayoutParams(
					ViewGroup.LayoutParams.FILL_PARENT, 64);

			TextView textView = new TextView(Bulletin2Activity.this);
			textView.setLayoutParams(lp);
			// Center the text vertically
			textView.setGravity(Gravity.CENTER_VERTICAL | Gravity.LEFT);
			// Set the text starting position
			textView.setPadding(36, 0, 0, 0);
			return textView;
		}

		@Override
		public View getChildView(int groupPosition, int childPosition, boolean isLastChild,
				View convertView, ViewGroup parent) {
			TextView textView = getGenericView();
			textView.setText(getChild(groupPosition, childPosition).toString());
			return textView;
		}

		@Override
		public Object getGroup(int groupPosition) {
			return groups[groupPosition];
		}

		@Override
		public int getGroupCount() {
			return groups.length;
		}

		@Override
		public long getGroupId(int groupPosition) {
			return groupPosition;
		}

		@Override
		public View getGroupView(int groupPosition, boolean isExpanded, View convertView,
				ViewGroup parent) {
			TextView textView = getGenericView();
			textView.setText(getGroup(groupPosition).toString());
			return textView;
		}

		@Override
		public boolean isChildSelectable(int groupPosition, int childPosition) {
			return true;
		}

		@Override
		public boolean hasStableIds() {
			return true;
		}
	}

	public class TextAdapter extends BaseAdapter {
		private Context mContext;

		public String[] baseArray = new String[65];
		public int baseIndex = 1;
		public int globalTreeNum = 0;
		public int lastTreeNum = 64;

		public TextAdapter(Context c) {
			mContext = c;

			mThumbIds = comments();
		}

		@Override
		public int getCount() {
			return mThumbIds.length;
		}

		@Override
		public Object getItem(int position) {
			return mThumbIds[position];
		}

		@Override
		public long getItemId(int position) {
			return 0;
		}

		// create a new ImageView for each item referenced by the Adapter
		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			TextView textView;
			if (convertView == null) {  // if it's not recycled, initialize some attributes
				textView = new TextView(mContext);
			} else {
				textView = (TextView) convertView;
			}

			textView.setText(mThumbIds[position]);
			return textView;
		}

		// references to our images
		private String[] mThumbIds = new String[65];

		public String[] comments ()
		{
			String comments = new String(BulletinFile.GetComments(headingNum, file));
			int count = 0;
			int currentCommentNum = 0; ;
			String temp = "";
			int numberOfComments = 1;
			int nextCommentNum = 0;
			String returnString[] = new String[65];
			int currentPosition = 1;

			temp = BulletinFile.GetHeading(headingNum, file);

			temp = String.valueOf(headingNum) + ":@" + temp;

			baseArray[0] = temp;

			if (comments.compareTo("") == 0)
			{
				return baseArray;
			}

			currentCommentNum = Integer.parseInt(comments.substring(comments.indexOf(":@") + 2, comments.indexOf(":", comments.indexOf(":@") + 2)));

			try
			{
				int index = comments.indexOf(":@" + String.valueOf(currentCommentNum) + ":");

				if(comments.indexOf(":@", index + 1) != -1)
				{
					temp = comments.substring(comments.indexOf(":@", index + 2), comments.indexOf(":", comments.indexOf(":@", index + 2) + 2)).replace(":", "");	
				}
				else
				{
					temp = comments.substring(index + 1, comments.indexOf(":" , index + 1));
				}
				temp = temp.replace("@", "");
				nextCommentNum = Integer.parseInt(temp);
			}
			catch (Exception e)
			{
				nextCommentNum = 0;
			}

			temp = comments;

			while (temp.indexOf(":@") != -1)
			{
				numberOfComments++;
				temp = temp.substring(temp.indexOf(":@") + 1, temp.length() - 1);
			}

			while (baseIndex < numberOfComments)
			{
				if (baseIndex + 1 == numberOfComments)  //last comment
				{
					nextCommentNum = 0;
				}
				else
				{
				}

				if (baseIndex < numberOfComments)
				{
					baseArray[baseIndex] = new String(BulletinFile.GetComment(headingNum, file, currentCommentNum).replace("\r", " "));

					if(baseArray[baseIndex] == "COMMENT_NOT_FOUND")
					{
						baseArray[baseIndex] = "";
						return baseArray;
					}

					baseIndex++;

					if (nextCommentNum > currentCommentNum)
					{
						globalTreeNum = nextCommentNum;
						newItem_Expanded(baseIndex, 1);
					}
				}
				else
				{

				}

				String header = baseArray[baseIndex - 1];

				if(header == null)
				{
					return baseArray;
				}

				while(header.startsWith("=>"))
				{
					header = header.replaceFirst("=>", "");
				}

				int headerNum = Integer.parseInt(header.substring(0, 2).replace(":", ""));
				currentCommentNum = headerNum;

				try
				{
					int index = comments.indexOf(":@" + String.valueOf(currentCommentNum) + ":");

					if(comments.indexOf(":@", index + 1) != -1)
					{
						temp = comments.substring(comments.indexOf(":@", index + 2), comments.indexOf(":", comments.indexOf(":@", index + 2) + 2)).replace(":", "");	
					}
					else
					{
						temp = comments.substring(index + 1, comments.indexOf(":" , index + 1));
					}
					
					temp = temp.replace("@", "");
					currentCommentNum = Integer.parseInt(temp);

					index = comments.indexOf(":@" + String.valueOf(currentCommentNum) + ":");

					if(comments.indexOf(":@", index + 1) != -1)
					{
						temp = comments.substring(comments.indexOf(":@", index + 2), comments.indexOf(":", comments.indexOf(":@", index + 2) + 2)).replace(":", "");	
					}
					else
					{
						temp = comments.substring(index + 1, comments.indexOf(":" , index + 1));
					}

					temp = temp.replace("@", "");
					nextCommentNum = Integer.parseInt(temp);
				}
				catch (Exception e)
				{
					nextCommentNum = 0;
				}
			}

			return baseArray;	
		}

		public int newItem_Expanded(int parentIndex, int depth)
		{
			String comments = BulletinFile.GetComments(headingNum, file);
			String comments2 = "";
			int count = 0;
			int currentCommentNum = 0; ;
			String temp = "";
			int numberOfComments = 1;
			int nextCommentNum = 0;
			String header = "";
			int headerNum = 0;
			int lastTreeNum2 = lastTreeNum;

			header = baseArray[baseIndex - 1];

			if(header == null)
			{
				baseIndex--;
				return headerNum;
			}

			while(header.startsWith("=>"))
			{
				header = header.replaceFirst("=>", "");
			}

			headerNum = Integer.parseInt(header.substring(0, 2).replace(":", ""));

			comments2 = comments.substring(comments.indexOf(header) + header.length() - 1, comments.length() - 1);  //ignore comments that are not relavant

			if (comments2.length() <= 1 || !comments2.contains(":@")) //end of comments
			{
				baseIndex--;
				return headerNum;
			}

			comments2 = comments2.replace("\r", " ");
			temp = comments2.substring(comments2.indexOf(":@") + 2, comments2.indexOf(":", comments2.indexOf(":@") + 2));
			temp = temp.replace(":", "");
			temp = temp.replace("@", "");
			currentCommentNum = Integer.parseInt(temp);

			try
			{
				int index = comments.indexOf(":@" + String.valueOf(currentCommentNum) + ":");

				if(comments.indexOf(":@", index + 1) != -1)
				{
					temp = comments.substring(comments.indexOf(":@", index + 2), comments.indexOf(":@", index + 2) + 4).replace(":", "");	
				}
				else
				{
					temp = comments.substring(index + 1, comments.indexOf(":" , index + 1));
				}

				temp = temp.replace("@", "");
				nextCommentNum = Integer.parseInt(temp);
				lastTreeNum = nextCommentNum;
			}
			catch (Exception e)
			{
				nextCommentNum = 0;
			}

			temp = comments;

			while (temp.indexOf(":@") != -1)
			{
				numberOfComments++;
				temp = temp.substring(temp.indexOf(":@") + 1, temp.length() - 1);
			}

			do
			{ 
				if (baseIndex < numberOfComments)
				{
					String bufferString = "";

					for(int x = 0; x < depth; x++)
					{
						bufferString += "=>";
					}

					String temp2 = BulletinFile.GetComment(headingNum, file, currentCommentNum).replace("\r", " ");

					if(temp2.equalsIgnoreCase("COMMENT_NOT_FOUND"))
					{
						baseIndex--;
						return headerNum;
					}

					baseArray[baseIndex] = bufferString.concat(temp2);

					baseIndex++;
					count++;

					globalTreeNum = nextCommentNum;

					if(nextCommentNum > currentCommentNum)
					{
						headerNum = currentCommentNum;

						lastTreeNum2 = newItem_Expanded(baseIndex, depth + 1);

						if(count == 1)
						{
							return 0;
						}
					}
					else
					{
					}

				}
				else
				{
					break;
				}

				if((currentCommentNum > nextCommentNum && headerNum > nextCommentNum) ||  (currentCommentNum < nextCommentNum && nextCommentNum > lastTreeNum))
				{
					return 0;
				}

				if (currentCommentNum > lastTreeNum && nextCommentNum > currentCommentNum)  //last comment
				{
					currentCommentNum = lastTreeNum;
				}
				else
				{
					currentCommentNum = nextCommentNum;
				}

				try
				{
					int index = comments.indexOf(":@" + String.valueOf(currentCommentNum) + ":");

					if(comments.indexOf(":@", index + 1) != -1)
					{
						temp = comments.substring(comments.indexOf(":@", index + 2), comments.indexOf(":@", index + 2) + 4).replace(":", "");	
					}
					else
					{
						temp = comments.substring(index + 1, comments.indexOf(":" , index + 1));
					}

					temp = temp.replace("@", "");
					nextCommentNum = Integer.parseInt(temp);
					lastTreeNum = nextCommentNum;
				}
				catch (Exception e)
				{
					nextCommentNum = 0;
				}


			}while (true);

			return headerNum;
		}
	}

	public class TextAdapter2 extends BaseAdapter {
		private Context mContext;

		public String[] baseArray = new String[65];
		public int baseIndex = 1;
		public int globalTreeNum = 0;
		public int lastTreeNum = 64;
				
		public TextAdapter2(Context c) {
			mContext = c;

			mThumbIds = comments();
		}

		@Override
		public int getCount() {
			return mThumbIds.length;
		}

		@Override
		public Object getItem(int position) {
			return mThumbIds[position];
		}

		@Override
		public long getItemId(int position) {
			return 0;
		}

		// create a new ImageView for each item referenced by the Adapter
		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			TextView textView;
			if (convertView == null) {  // if it's not recycled, initialize some attributes
				textView = new TextView(mContext);
			} else {
				textView = (TextView) convertView;
			}

			textView.setText(mThumbIds[position]);
			return textView;
		}

		// references to our images
		private String[] mThumbIds = new String[65];

		public String[] comments ()
		{
			String comments = new String(BulletinFile.GetComments(headingNum, new String (file)));
			int count = 0;
			int currentCommentNum = 0; ;
			String temp = "";
			int numberOfComments = 1;
			int nextCommentNum = 0;
			String returnString[] = new String[65];
			int currentPosition = 1;
			
			temp = new String(BulletinFile.GetHeading(headingNum, new String(file)));

			temp = String.valueOf(headingNum) + ":@" + temp;

			baseArray[0] = temp;

			if (comments.compareTo("") == 0)
			{
				return baseArray;
			}

			currentCommentNum = Integer.parseInt(comments.substring(comments.indexOf(":@") + 2, comments.indexOf(":", comments.indexOf(":@") + 2)));

			try
			{
				int index = comments.indexOf(":@" + String.valueOf(currentCommentNum) + ":");

				if(comments.indexOf(":@", index + 1) != -1)
				{
					temp = comments.substring(comments.indexOf(":@", index + 2), comments.indexOf(":", comments.indexOf(":@", index + 2) + 2)).replace(":", "");	
				}
				else
				{
					temp = comments.substring(index + 1, comments.indexOf(":" , index + 1));
				}
				
				temp = temp.replace("@", "");
				nextCommentNum = Integer.parseInt(temp);
			}
			catch (Exception e)
			{
				nextCommentNum = 0;
			}

			temp = comments;

			while (temp.indexOf(":@") != -1)
			{
				numberOfComments++;
				temp = temp.substring(temp.indexOf(":@") + 1, temp.length() - 1);
			}

			while (baseIndex < numberOfComments)
			{
				if (baseIndex + 1 == numberOfComments)  //last comment
				{
					nextCommentNum = 0;
				}
				else
				{
				}

				if (baseIndex < numberOfComments)
				{
					baseArray[baseIndex] = new String(BulletinFile.GetComment(headingNum, new String(file), currentCommentNum).replace("\r", " "));

					if(baseArray[baseIndex] == "COMMENT_NOT_FOUND")
					{
						baseArray[baseIndex] = "";
						return baseArray;
					}

					baseIndex++;

					if (nextCommentNum > currentCommentNum)
					{
						globalTreeNum = nextCommentNum;
						newItem_Expanded(baseIndex, 1);
					}
				}
				else
				{

				}

				String header = baseArray[baseIndex - 1];

				if(header == null)
				{
					return baseArray;
				}

				while(header.startsWith("=>"))
				{
					header = header.replaceFirst("=>", "");
				}

				int headerNum = Integer.parseInt(header.substring(0, 2).replace(":", ""));
				currentCommentNum = headerNum;

				try
				{
					int index = comments.indexOf(":@" + String.valueOf(currentCommentNum) + ":");

					if(comments.indexOf(":@", index + 1) != -1)
					{
						temp = comments.substring(comments.indexOf(":@", index + 2), comments.indexOf(":", comments.indexOf(":@", index + 2) + 2)).replace(":", "");	
					}
					else
					{
						temp = comments.substring(index + 1, comments.indexOf(":" , index + 1));
					}
					temp = temp.replace("@", "");
					currentCommentNum = Integer.parseInt(temp);

					index = comments.indexOf(":@" + String.valueOf(currentCommentNum) + ":");

					if(comments.indexOf(":@", index + 1) != -1)
					{
						temp = comments.substring(comments.indexOf(":@", index + 2), comments.indexOf(":", comments.indexOf(":@", index + 2) + 2)).replace(":", "");	
					}
					else
					{
						temp = comments.substring(index + 1, comments.indexOf(":" , index + 1));
					}

					temp = temp.replace("@", "");
					nextCommentNum = Integer.parseInt(temp);
				}
				catch (Exception e)
				{
					nextCommentNum = 0;
				}
			}
			
			int depth = 0;
			int lastDepth = 0;
			//expandedTexts = baseArray.clone();
			
			for(int x = 0; x < 65 && baseArray[x] != null; x++)
			{
				//int lastDepth = depth;
				depth = 0;
				int index = 0;
				
				while(index < baseArray[x].indexOf(":"))
				{
					index = baseArray[x].indexOf("=>", index + 1);
					
					if(index != -1 && index < baseArray[x].indexOf(":"))
					{
						depth++;
					}
					else
					{
						break;
					}
				}

				if(baseArray[x].endsWith("SHOULD_BE_COLLAPSED") || (x != 0 && baseArray[x - 1].startsWith("+") && depth != lastDepth && depth != 0))
				{
					for(int y = x; y < 65 && baseArray[y] != null; y++)
					{
						baseArray[y] = baseArray[y + 1];
					}
					
					x--;
				}
				else
				{
					lastDepth = depth;
				}
			}
			
			return baseArray;
		}

		public int newItem_Expanded(int parentIndex, int depth)
		{
			String comments = new String(BulletinFile.GetComments(headingNum, file));
			String comments2 = "";
			int count = 0;
			int currentCommentNum = 0; ;
			String temp = "";
			int numberOfComments = 1;
			int nextCommentNum = 0;
			String header = "";
			int headerNum = 0;
			int lastTreeNum2 = lastTreeNum;
			int lastDepth = depth;
			int lastTag = -1;
			
			
			if(baseIndex != numberOfComments)
			{
				String temp3 = null;
    
				if(expandedTag[baseIndex - 1] == -1)
				{
					expandedTag[baseIndex - 1] = 0;
				}
				
				if(expandedTag[baseIndex - 1] == 1)
				{
					temp3 = new String("-");
					baseArray[baseIndex - 1] = temp3.concat(baseArray[baseIndex - 1]);
				}
				else if(expandedTag[baseIndex - 1] == 0)
				{
					temp3 = new String("+");
					baseArray[baseIndex - 1] = temp3.concat(baseArray[baseIndex - 1]);
				}
			}
			
			lastTag = expandedTag[baseIndex - 1];
			
			header = baseArray[baseIndex - 1];
			
			if(header == null)
			{
				baseIndex--;
				return headerNum;
			}

			if(header.startsWith("+"))
			{
				header = header.substring(1);
			}
			else if(header.startsWith("-"))
			{
				header = header.substring(1);
			}
			
			while(header.startsWith("=>"))
			{
				header = header.replaceFirst("=>", "");
			}

			headerNum = Integer.parseInt(header.substring(0, 2).replace(":", ""));

			if(header.endsWith("SHOULD_BE_COLLAPSED"))
			{
				header = header.substring(0, header.length() - 19);
			}
			
			comments2 = comments.substring(comments.indexOf(header) + header.length() - 1, comments.length() - 1);  //ignore comments that are not relevant

			if (comments2.length() <= 1 || !comments2.contains(":@")) //end of comments
			{
				baseIndex--;
				return headerNum;
			}

			comments2 = comments2.replace("\r", " ");
			temp = comments2.substring(comments2.indexOf(":@") + 2, comments2.indexOf(":", comments2.indexOf(":@") + 2));
			temp = temp.replace(":", "");
			temp = temp.replace("@", "");
			currentCommentNum = Integer.parseInt(temp);

			try
			{
				int index = comments.indexOf(":@" + String.valueOf(currentCommentNum) + ":");

				if(comments.indexOf(":@", index + 1) != -1)
				{
					temp = comments.substring(comments.indexOf(":@", index + 2), comments.indexOf(":@", index + 2) + 4).replace(":", "");	
				}
				else
				{
					temp = comments.substring(index + 1, comments.indexOf(":" , index + 1));
				}

				temp = temp.replace("@", "");
				nextCommentNum = Integer.parseInt(temp);
				lastTreeNum = nextCommentNum;
			}
			catch (Exception e)
			{
				nextCommentNum = 0;
			}

			temp = comments;

			while (temp.indexOf(":@") != -1)
			{
				numberOfComments++;
				temp = temp.substring(temp.indexOf(":@") + 1, temp.length() - 1);
			}

			do
			{ 
				if (baseIndex < numberOfComments)
				{
					StringBuffer bufferString = new StringBuffer();

					for(int x = 0; x < depth; x++)
					{
						String temp4 = new String("=>");
						bufferString = bufferString.append(temp4);
					}

					StringBuffer temp2 = new StringBuffer();
					temp2.append(BulletinFile.GetComment(headingNum, String.copyValueOf((file.toCharArray())), currentCommentNum).replace("\r", " "));

					if(temp2.toString().equalsIgnoreCase("COMMENT_NOT_FOUND"))
					{
						baseIndex--;
						return headerNum;
					}
					
					if(lastTag == 0)
					{
 //   					baseArray[baseIndex] = new String(bufferString.append(temp2).append("SHOULD_BE_COLLAPSED").toString());
    					
    					String collapsedString = "SHOULD_BE_COLLAPSED";
    					
    					char[] chars = new char[10000];
    					
    					for(int x = 0; x < bufferString.length(); x++)
    					{
    						chars[x] = bufferString.charAt(x);
    					}
    					
    					for(int x = bufferString.length(); x < temp2.length() + bufferString.length(); x++)
    					{
    						chars[x] = temp2.charAt(x - bufferString.length());
    					}

    					for(int x = temp2.length() + bufferString.length(); x < collapsedString.length() + temp2.length() + bufferString.length(); x++)
    					{
    						chars[x] = collapsedString.charAt(x - bufferString.length() - temp2.length());
    					}
			 					   					
						baseArray[baseIndex] = String.copyValueOf(chars).trim();
					}
					else
					{
						baseArray[baseIndex] = bufferString.append(temp2).toString();
					}

					//baseArray[baseIndex] = bufferString.concat(temp2);

					baseIndex++;
					count++;

					globalTreeNum = nextCommentNum;

					if(nextCommentNum > currentCommentNum)
					{
						headerNum = currentCommentNum;

						lastTreeNum2 = newItem_Expanded(baseIndex, depth + 1);

						if(count == 1)
						{
							return 0;
						}
					}
					else
					{
					}

				}
				else
				{
					break;
				}

				if((currentCommentNum > nextCommentNum && headerNum > nextCommentNum) ||  (currentCommentNum < nextCommentNum && nextCommentNum > lastTreeNum))
				{
					return 0;
				}

				if (currentCommentNum > lastTreeNum && nextCommentNum > currentCommentNum)  //last comment
				{
					currentCommentNum = lastTreeNum;
				}
				else
				{
					currentCommentNum = nextCommentNum;
				}

				try
				{
					int index = comments.indexOf(":@" + String.valueOf(currentCommentNum) + ":");

					if(comments.indexOf(":@", index + 1) != -1)
					{
						temp = comments.substring(comments.indexOf(":@", index + 2), comments.indexOf(":@", index + 2) + 4).replace(":", "");	
					}
					else
					{
						temp = comments.substring(index + 1, comments.indexOf(":" , index + 1));
					}

					temp = temp.replace("@", "");
					nextCommentNum = Integer.parseInt(temp);
					lastTreeNum = nextCommentNum;
				}
				catch (Exception e)
				{
					nextCommentNum = 0;
				}


			}while (true);

			return headerNum;
		}
	}
}
