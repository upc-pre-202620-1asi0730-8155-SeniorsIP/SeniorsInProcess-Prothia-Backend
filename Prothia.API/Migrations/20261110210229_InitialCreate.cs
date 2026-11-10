using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Prothia.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "alerts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    patient_id = table.Column<int>(type: "int", nullable: false),
                    patient_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    deviation_type = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    recorded_value = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    threshold = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    severity = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    occurred_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    resolved_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_alerts", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clinic_subscriptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    plan_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    plan_type = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    annual_fee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    monthly_equivalent = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    start_date = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    renewal_date = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    used_quota = table.Column<int>(type: "int", nullable: false),
                    total_quota = table.Column<int>(type: "int", nullable: false),
                    payment_method_brand = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    payment_method_card_title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    payment_method_expiry = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    payment_method_auto_renew = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_clinic_subscriptions", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clinical_messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    sender = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    sender_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    sender_role = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    text = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    timestamp = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    is_system = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_clinical_messages", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clinical_reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    report_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    issue_date = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    clinic_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    clinic_unit = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    patient_id = table.Column<int>(type: "int", nullable: false),
                    patient_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    dni = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    diagnosis = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    prosthesis_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    evaluation_period = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    summary = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    medical_recommendation = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    therapist_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    therapist_role = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    therapist_registry = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_clinical_reports", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "clinical_transmissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    patient_id = table.Column<int>(type: "int", nullable: false),
                    patient_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    patient_dni = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    workshop_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    shared_modules = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    technical_notes = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    status_label = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    summary = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    created_at_date = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_clinical_transmissions", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "electronic_invoices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    invoice_number = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    issue_date = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    concept = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    status_label = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    download_url = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_electronic_invoices", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "exercises",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    focus = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_exercises", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "gait_sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    patient_id = table.Column<int>(type: "int", nullable: false),
                    recorded_at = table.Column<DateTimeOffset>(type: "datetime", nullable: false),
                    duration_minutes = table.Column<int>(type: "int", nullable: false),
                    steps = table.Column<int>(type: "int", nullable: false),
                    cadence = table.Column<int>(type: "int", nullable: false),
                    symmetry_percent = table.Column<int>(type: "int", nullable: false),
                    alerts_count = table.Column<int>(type: "int", nullable: false),
                    impact_peak_newtons = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_gait_sessions", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "maintenance_schedules",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    serial_number = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    patient_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    scheduled_date = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    intervention_type = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    technician_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_maintenance_schedules", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "mechanical_alerts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    prosthesis_id = table.Column<int>(type: "int", nullable: false),
                    serial_number = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    cycles = table.Column<int>(type: "int", nullable: false),
                    cycle_limit = table.Column<int>(type: "int", nullable: false),
                    patient_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    k_level = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    component = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    severity = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    occurred_at = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_mechanical_alerts", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    full_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    dni = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    e_amputation = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    ek_level = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    prosthesis_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    adherence = table.Column<int>(type: "int", nullable: false),
                    symmetry = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_patients", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "prescriptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    patient_id = table.Column<int>(type: "int", nullable: false),
                    phase_number = table.Column<int>(type: "int", nullable: false),
                    phase_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    duration_weeks = table.Column<int>(type: "int", nullable: false),
                    frequency_per_week = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_prescriptions", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "prostheses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    serial_number = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    patient_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    clinic_name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    accumulated_cycles = table.Column<int>(type: "int", nullable: false),
                    cycle_limit = table.Column<int>(type: "int", nullable: false),
                    last_service = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    battery_level = table.Column<int>(type: "int", nullable: false),
                    sensor_status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    k_level = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    socket_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    knee_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    foot_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_prostheses", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "threshold_profiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    patient_id = table.Column<int>(type: "int", nullable: false),
                    pelvic_tilt = table.Column<int>(type: "int", nullable: false),
                    asymmetry = table.Column<int>(type: "int", nullable: false),
                    cadence = table.Column<int>(type: "int", nullable: false),
                    impact_peak = table.Column<double>(type: "double", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_threshold_profiles", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "report_indicators",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    clinical_report_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    value = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normal_range = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    evaluation = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_report_indicators", x => x.id);
                    table.ForeignKey(
                        name: "f_k_report_indicators_clinical_reports_clinical_report_id",
                        column: x => x.clinical_report_id,
                        principalTable: "clinical_reports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "prescribed_exercises",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    prescription_id = table.Column<int>(type: "int", nullable: false),
                    exercise_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    sets = table.Column<int>(type: "int", nullable: true),
                    reps = table.Column<int>(type: "int", nullable: true),
                    rest_seconds = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_prescribed_exercises", x => x.id);
                    table.ForeignKey(
                        name: "f_k_prescribed_exercises_prescriptions_prescription_id",
                        column: x => x.prescription_id,
                        principalTable: "prescriptions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_alerts_patient_id_status",
                table: "alerts",
                columns: new[] { "patient_id", "status" });

            migrationBuilder.CreateIndex(
                name: "i_x_clinical_reports_patient_id",
                table: "clinical_reports",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "i_x_clinical_reports_report_code",
                table: "clinical_reports",
                column: "report_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_clinical_transmissions_patient_id",
                table: "clinical_transmissions",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "i_x_electronic_invoices_invoice_number",
                table: "electronic_invoices",
                column: "invoice_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_exercises_name",
                table: "exercises",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_gait_sessions_patient_id_recorded_at",
                table: "gait_sessions",
                columns: new[] { "patient_id", "recorded_at" });

            migrationBuilder.CreateIndex(
                name: "i_x_mechanical_alerts_prosthesis_id_status",
                table: "mechanical_alerts",
                columns: new[] { "prosthesis_id", "status" });

            migrationBuilder.CreateIndex(
                name: "i_x_patients_dni",
                table: "patients",
                column: "dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_prescribed_exercises_prescription_id",
                table: "prescribed_exercises",
                column: "prescription_id");

            migrationBuilder.CreateIndex(
                name: "i_x_prescriptions_patient_id",
                table: "prescriptions",
                column: "patient_id");

            migrationBuilder.CreateIndex(
                name: "i_x_prostheses_serial_number",
                table: "prostheses",
                column: "serial_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_report_indicators_clinical_report_id",
                table: "report_indicators",
                column: "clinical_report_id");

            migrationBuilder.CreateIndex(
                name: "i_x_threshold_profiles_patient_id",
                table: "threshold_profiles",
                column: "patient_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alerts");

            migrationBuilder.DropTable(
                name: "clinic_subscriptions");

            migrationBuilder.DropTable(
                name: "clinical_messages");

            migrationBuilder.DropTable(
                name: "clinical_transmissions");

            migrationBuilder.DropTable(
                name: "electronic_invoices");

            migrationBuilder.DropTable(
                name: "exercises");

            migrationBuilder.DropTable(
                name: "gait_sessions");

            migrationBuilder.DropTable(
                name: "maintenance_schedules");

            migrationBuilder.DropTable(
                name: "mechanical_alerts");

            migrationBuilder.DropTable(
                name: "patients");

            migrationBuilder.DropTable(
                name: "prescribed_exercises");

            migrationBuilder.DropTable(
                name: "prostheses");

            migrationBuilder.DropTable(
                name: "report_indicators");

            migrationBuilder.DropTable(
                name: "threshold_profiles");

            migrationBuilder.DropTable(
                name: "prescriptions");

            migrationBuilder.DropTable(
                name: "clinical_reports");
        }
    }
}
