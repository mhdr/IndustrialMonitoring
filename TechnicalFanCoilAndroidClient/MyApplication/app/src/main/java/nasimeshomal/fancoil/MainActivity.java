package nasimeshomal.fancoil;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.RadioButton;
import android.widget.ToggleButton;

public class MainActivity extends AppCompatActivity {

    ToggleButton toggleButtonMotor1;
    ToggleButton toggleButtonMotor2;

    RadioButton radioButtonMotor1Speed1;
    RadioButton radioButtonMotor1Speed2;
    RadioButton radioButtonMotor1Speed3;

    RadioButton radioButtonMotor2Speed1;
    RadioButton radioButtonMotor2Speed2;
    RadioButton radioButtonMotor2Speed3;

    Button buttonFetch;
    Button buttonSave;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);


        toggleButtonMotor1= (ToggleButton) findViewById(R.id.toggleButtonMotor1);
        toggleButtonMotor2= (ToggleButton) findViewById(R.id.toggleButtonMotor2);
        radioButtonMotor1Speed1= (RadioButton) findViewById(R.id.radioButtonMotor1Speed1);
        radioButtonMotor1Speed2= (RadioButton) findViewById(R.id.radioButtonMotor1Speed2);
        radioButtonMotor1Speed3= (RadioButton) findViewById(R.id.radioButtonMotor1Speed3);
        radioButtonMotor2Speed1= (RadioButton) findViewById(R.id.radioButtonMotor2Speed1);
        radioButtonMotor2Speed2= (RadioButton) findViewById(R.id.radioButtonMotor2Speed2);
        radioButtonMotor2Speed3= (RadioButton) findViewById(R.id.radioButtonMotor2Speed3);
        buttonFetch= (Button) findViewById(R.id.buttonFetch);
        buttonSave=(Button)findViewById(R.id.buttonSave);


        toggleButtonMotor1.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (toggleButtonMotor1.isChecked())
                {
                    radioButtonMotor1Speed1.setEnabled(true);
                    radioButtonMotor1Speed2.setEnabled(true);
                    radioButtonMotor1Speed3.setEnabled(true);
                }
                else
                {
                    radioButtonMotor1Speed1.setEnabled(false);
                    radioButtonMotor1Speed2.setEnabled(false);
                    radioButtonMotor1Speed3.setEnabled(false);
                }
            }
        });

        toggleButtonMotor2.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (toggleButtonMotor2.isChecked())
                {
                    radioButtonMotor2Speed1.setEnabled(true);
                    radioButtonMotor2Speed2.setEnabled(true);
                    radioButtonMotor2Speed3.setEnabled(true);
                }
                else
                {
                    radioButtonMotor2Speed1.setEnabled(false);
                    radioButtonMotor2Speed2.setEnabled(false);
                    radioButtonMotor2Speed3.setEnabled(false);
                }

            }
        });

        buttonFetch.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

            }
        });

        buttonSave.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

            }
        });
    }
}
